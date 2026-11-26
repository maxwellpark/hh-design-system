extends Control
## The Hooton Heroics studio splash for Godot 4. Brand~/README.md is the spec.
##
## Usage from the game's first scene:
##     const HHSplash := preload("res://addons/hh_studio/hh_splash.gd")
##     var splash := HHSplash.create(load("res://art/russell-studio-host.png"))
##     add_child(splash)
##     await splash.finished
## Pass null for no host.

signal finished

const Tokens := preload("res://addons/hh_studio/hh_tokens.gd")
const LOGO := preload("res://addons/hh_studio/hh-logo.png")

@export var host_texture: Texture2D
@export var reduced_motion := false

var elapsed := 0.0
var done := false


static func create(host: Texture2D = null, cut_instead_of_fade := false) -> Control:
	var splash: Control = load("res://addons/hh_studio/hh_splash.gd").new()
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


func _ready() -> void:
	set_anchors_and_offsets_preset(Control.PRESET_FULL_RECT)
	mouse_filter = Control.MOUSE_FILTER_STOP
	var field := ColorRect.new()
	field.name = "Field"
	field.color = Tokens.BG
	field.set_anchors_and_offsets_preset(Control.PRESET_FULL_RECT)
	field.mouse_filter = Control.MOUSE_FILTER_IGNORE
	add_child(field)
	var content := Control.new()
	content.name = "Content"
	content.set_anchors_and_offsets_preset(Control.PRESET_FULL_RECT)
	content.mouse_filter = Control.MOUSE_FILTER_IGNORE
	add_child(content)
	content.add_child(_texture("Logo", LOGO))
	if host_texture != null:
		content.add_child(_texture("Host", host_texture))
	var wordmark := Label.new()
	wordmark.name = "Wordmark"
	wordmark.text = Tokens.SPLASH_WORDMARK
	wordmark.add_theme_color_override("font_color", Tokens.TEXT)
	wordmark.horizontal_alignment = HORIZONTAL_ALIGNMENT_CENTER
	content.add_child(wordmark)
	resized.connect(_layout)
	_layout()
	content.modulate.a = alpha_at(0.0, reduced_motion)


func _process(delta: float) -> void:
	advance(delta)


## Moves the splash on by a time step; the game loop calls it each frame and tests call it directly.
func advance(delta: float) -> void:
	if done:
		return
	elapsed += delta
	if elapsed >= total_seconds():
		finish()
		return
	get_node("Content").modulate.a = alpha_at(elapsed, reduced_motion)


func _input(event: InputEvent) -> void:
	if skips(event):
		get_viewport().set_input_as_handled()
		finish()


static func skips(event: InputEvent) -> bool:
	if event is InputEventKey or event is InputEventMouseButton or event is InputEventJoypadButton:
		return event.pressed and not event.is_echo()
	return event is InputEventScreenTouch and event.pressed


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
	var content := get_node_or_null("Content")
	if content == null:
		return
	var short_side := minf(size.x, size.y)
	var centre := size / 2.0
	var logo_size := Vector2.ONE * short_side * Tokens.SPLASH_LOGO_HEIGHT
	var logo: TextureRect = content.get_node("Logo")
	logo.position = centre - logo_size / 2.0
	logo.size = logo_size
	var wordmark: Label = content.get_node("Wordmark")
	wordmark.add_theme_font_size_override("font_size", maxi(16, int(short_side * 0.045)))
	wordmark.position = Vector2(0, logo.position.y + logo_size.y + short_side * 0.04)
	wordmark.size = Vector2(size.x, short_side * 0.08)
	var host: TextureRect = content.get_node_or_null("Host")
	if host != null:
		var host_size := Vector2.ONE * short_side * Tokens.SPLASH_HOST_HEIGHT
		host.size = host_size
		host.position = Vector2(logo.position.x - host_size.x - short_side * 0.04, centre.y - host_size.y / 2.0)
