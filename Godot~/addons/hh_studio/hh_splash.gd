extends CanvasLayer
## The Hooton Heroics studio splash for Godot 4. Brand~/README.md is the spec.
##
## Usage from the game's first scene, whatever its root (Node, Node2D or Control):
##     const HHSplash := preload("res://addons/hh_studio/hh_splash.gd")
##     var splash := HHSplash.create(load("res://art/russell-studio-host.png"))
##     add_child(splash)
##     await splash.finished
## Pass null for no host.

signal finished

const Tokens := preload("res://addons/hh_studio/hh_tokens.gd")
const LOGO := preload("res://addons/hh_studio/hh-logo.png")

## A hitch longer than this (a synchronous load behind the splash) counts as one ordinary frame, so it
## cannot skip the fade or the splash, while a steady low frame rate still runs in real time.
const HITCH := 0.25
const HITCH_STEP := 1.0 / 30.0
## Above every game layer, so no game UI draws over it.
const LAYER := 128
const WORDMARK_SPACING := 4

@export var host_texture: Texture2D
@export var reduced_motion := false

var elapsed := 0.0
var done := false
var frame: Control


static func create(host: Texture2D = null, cut_instead_of_fade := false) -> CanvasLayer:
	var splash: CanvasLayer = load("res://addons/hh_studio/hh_splash.gd").new()
	splash.host_texture = host
	splash.reduced_motion = cut_instead_of_fade
	return splash


static func total_seconds() -> float:
	return Tokens.SPLASH_FADE_IN + Tokens.SPLASH_HOLD + Tokens.SPLASH_FADE_OUT


## Opacity at a time since the start: a fade in, a hold and a fade out, or a hard cut with reduced motion.
static func alpha_at(seconds: float, cut := false) -> float:
	if seconds < 0.0 or seconds >= total_seconds():
		return 0.0
	if cut:
		return 1.0
	if seconds < Tokens.SPLASH_FADE_IN:
		return seconds / Tokens.SPLASH_FADE_IN
	var fade_out_from := Tokens.SPLASH_FADE_IN + Tokens.SPLASH_HOLD
	if seconds > fade_out_from:
		return 1.0 - (seconds - fade_out_from) / Tokens.SPLASH_FADE_OUT
	return 1.0


## Rects for the mark, the wordmark and the host on a screen. The host stands left of the mark when it
## fits with clear space; on narrower screens it stands above, shrunk to fit if it must. Clear space is a
## quarter of the mark.
static func layout_for(screen: Vector2, with_host: bool) -> Dictionary:
	var short_side := minf(screen.x, screen.y)
	var mark := Vector2.ONE * short_side * Tokens.SPLASH_LOGO_HEIGHT
	var clear := mark.y / 4.0
	var host := Vector2.ONE * short_side * Tokens.SPLASH_HOST_HEIGHT if with_host else Vector2.ZERO
	var wordmark_height := short_side * 0.08
	var centre := screen / 2.0
	var mark_pos := centre - mark / 2.0
	var host_pos := Vector2.ZERO
	var beside := with_host and mark_pos.x - clear - host.x >= clear
	if with_host and beside:
		host_pos = Vector2(mark_pos.x - clear - host.x, centre.y - host.y / 2.0)
	elif with_host:
		# Stacked, the host gives up height first so the whole column stays inside 90% of the screen.
		var rest := clear + mark.y + clear + wordmark_height
		host = Vector2.ONE * clampf(screen.y * 0.9 - rest, 0.0, host.y)
		var stack := host.y + rest
		host_pos = Vector2(centre.x - host.x / 2.0, centre.y - stack / 2.0)
		mark_pos = Vector2(mark_pos.x, host_pos.y + host.y + clear)
	return {
		"mark": Rect2(mark_pos, mark),
		"wordmark": Rect2(Vector2(0, mark_pos.y + mark.y + clear), Vector2(screen.x, wordmark_height)),
		"host": Rect2(host_pos, host),
		"beside": beside,
		"font_size": maxi(16, int(short_side * 0.045)),
	}


func _ready() -> void:
	layer = LAYER
	frame = Control.new()
	frame.name = "Frame"
	frame.mouse_filter = Control.MOUSE_FILTER_STOP
	add_child(frame)
	frame.set_anchors_and_offsets_preset(Control.PRESET_FULL_RECT)
	var field := ColorRect.new()
	field.name = "Field"
	field.color = Tokens.BG
	field.mouse_filter = Control.MOUSE_FILTER_IGNORE
	frame.add_child(field)
	field.set_anchors_and_offsets_preset(Control.PRESET_FULL_RECT)
	var content := Control.new()
	content.name = "Content"
	content.mouse_filter = Control.MOUSE_FILTER_IGNORE
	frame.add_child(content)
	content.set_anchors_and_offsets_preset(Control.PRESET_FULL_RECT)
	content.add_child(_texture("Logo", LOGO))
	if host_texture != null:
		content.add_child(_texture("Host", host_texture))
	var wordmark := Label.new()
	wordmark.name = "Wordmark"
	wordmark.text = Tokens.SPLASH_WORDMARK
	wordmark.add_theme_color_override("font_color", Tokens.TEXT)
	wordmark.horizontal_alignment = HORIZONTAL_ALIGNMENT_CENTER
	wordmark.mouse_filter = Control.MOUSE_FILTER_IGNORE
	content.add_child(wordmark)
	# The game's theme font, letter-spaced; read once the label is in the tree so the project theme applies.
	var spaced := FontVariation.new()
	spaced.base_font = wordmark.get_theme_font("font")
	spaced.spacing_glyph = WORDMARK_SPACING
	wordmark.add_theme_font_override("font", spaced)
	frame.resized.connect(_layout)
	_layout()
	content.modulate.a = alpha_at(0.0, reduced_motion)


func _process(delta: float) -> void:
	advance(delta)


## Moves the splash on by a time step, a hitch counting as one frame; the game loop calls it each frame
## and tests call it directly.
func advance(delta: float) -> void:
	if done:
		return
	elapsed += HITCH_STEP if delta > HITCH else delta
	if elapsed >= total_seconds():
		finish()
		return
	frame.get_node("Content").modulate.a = alpha_at(elapsed, reduced_motion)


func _input(event: InputEvent) -> void:
	if skips(event):
		get_viewport().set_input_as_handled()
		finish()


static func skips(event: InputEvent) -> bool:
	if event is InputEventMouseButton:
		var wheel: bool = event.button_index in [MOUSE_BUTTON_WHEEL_UP, MOUSE_BUTTON_WHEEL_DOWN, MOUSE_BUTTON_WHEEL_LEFT, MOUSE_BUTTON_WHEEL_RIGHT]
		return event.pressed and not wheel
	if event is InputEventKey:
		return event.pressed and not event.is_echo()
	if event is InputEventJoypadButton or event is InputEventScreenTouch:
		return event.pressed
	return false


func finish() -> void:
	if done:
		return
	done = true
	finished.emit()
	queue_free()


func _texture(node_name: String, texture: Texture2D) -> TextureRect:
	var rect := TextureRect.new()
	rect.name = node_name
	rect.texture = texture
	rect.expand_mode = TextureRect.EXPAND_IGNORE_SIZE
	rect.stretch_mode = TextureRect.STRETCH_KEEP_ASPECT_CENTERED
	rect.mouse_filter = Control.MOUSE_FILTER_IGNORE
	return rect


func _layout() -> void:
	var content := frame.get_node("Content")
	var host: TextureRect = content.get_node_or_null("Host")
	var rects := layout_for(frame.size, host != null)
	var logo: TextureRect = content.get_node("Logo")
	logo.position = rects["mark"].position
	logo.size = rects["mark"].size
	var wordmark: Label = content.get_node("Wordmark")
	wordmark.add_theme_font_size_override("font_size", rects["font_size"])
	wordmark.position = rects["wordmark"].position
	wordmark.size = rects["wordmark"].size
	if host != null:
		host.position = rects["host"].position
		host.size = rects["host"].size
