extends "res://tests/test_case.gd"

const HHSplash := preload("res://addons/hh_studio/hh_splash.gd")
const Tokens := preload("res://addons/hh_studio/hh_tokens.gd")


func test_the_splash_lasts_two_seconds_and_fades_in_holds_and_fades_out() -> bool:
	check_eq(HHSplash.total_seconds(), 2.0)
	check_eq(HHSplash.alpha_at(0.0), 0.0)
	check(is_equal_approx(HHSplash.alpha_at(Tokens.SPLASH_FADE_IN / 2.0), 0.5), "half way through the fade in")
	check_eq(HHSplash.alpha_at(1.0), 1.0, "holding")
	check(HHSplash.alpha_at(1.9) < 0.5, "fading out")
	check_eq(HHSplash.alpha_at(2.0), 0.0, "gone at the end")
	return done()


func test_reduced_motion_cuts_instead_of_fading() -> bool:
	check_eq(HHSplash.alpha_at(0.01, true), 1.0)
	check_eq(HHSplash.alpha_at(1.99, true), 1.0)
	check_eq(HHSplash.alpha_at(2.0, true), 0.0)
	return done()


func test_it_finishes_once_after_its_time_and_frees_itself() -> bool:
	var splash: Control = HHSplash.create()
	Engine.get_main_loop().root.add_child(splash)
	var count := [0]
	splash.finished.connect(func(): count[0] += 1)
	splash.advance(1.0)
	check_eq(count[0], 0, "still showing at one second")
	splash.advance(1.1)
	splash.advance(0.5)
	check_eq(count[0], 1, "finished exactly once")
	check(splash.is_queued_for_deletion(), "freed after finishing")
	return done()


func test_any_press_skips_but_a_release_or_key_repeat_does_not() -> bool:
	var key := InputEventKey.new()
	key.pressed = true
	check(HHSplash.skips(key), "a key press")
	key.echo = true
	check(not HHSplash.skips(key), "a held key's repeat")
	var click := InputEventMouseButton.new()
	click.pressed = false
	check(not HHSplash.skips(click), "a mouse release")
	click.pressed = true
	check(HHSplash.skips(click), "a click")
	var touch := InputEventScreenTouch.new()
	touch.pressed = true
	check(HHSplash.skips(touch), "a touch")
	var pad := InputEventJoypadButton.new()
	pad.pressed = true
	check(HHSplash.skips(pad), "a pad button")
	check(not HHSplash.skips(InputEventMouseMotion.new()), "moving the mouse")
	return done()


func test_layout_puts_the_host_left_of_the_mark_on_the_brand_field() -> bool:
	var host := ImageTexture.create_from_image(Image.create(8, 8, false, Image.FORMAT_RGBA8))
	var splash: Control = HHSplash.create(host)
	Engine.get_main_loop().root.add_child(splash)
	# Full-rect anchors would override a set size; a fixed 720p frame stands in for the screen.
	splash.set_anchors_preset(Control.PRESET_TOP_LEFT)
	splash.size = Vector2(1280, 720)
	var logo: TextureRect = splash.get_node("Content/Logo")
	var placed_host: TextureRect = splash.get_node("Content/Host")
	check_eq(splash.get_node("Field").color, Tokens.BG)
	check(is_equal_approx(logo.size.y, 720 * Tokens.SPLASH_LOGO_HEIGHT), "mark height follows the token")
	check(placed_host.position.x + placed_host.size.x < logo.position.x, "host stands left of the mark")
	check_eq(splash.get_node("Content/Wordmark").text, "HOOTON HEROICS")
	splash.free()
	var bare: Control = HHSplash.create()
	Engine.get_main_loop().root.add_child(bare)
	check(bare.get_node_or_null("Content/Host") == null, "no host when none is given")
	bare.free()
	return done()
