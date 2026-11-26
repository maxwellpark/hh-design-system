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
	var splash: CanvasLayer = HHSplash.create()
	Engine.get_main_loop().root.add_child(splash)
	var count := [0]
	splash.finished.connect(func(): count[0] += 1)
	for _frame in 30:
		splash.advance(1.0 / 30.0)
	check_eq(count[0], 0, "still showing at one second")
	for _frame in 40:
		splash.advance(1.0 / 30.0)
	check_eq(count[0], 1, "finished exactly once")
	check(splash.is_queued_for_deletion(), "freed after finishing")
	return done()


func test_a_steady_twenty_frames_a_second_still_takes_two_seconds() -> bool:
	var splash: CanvasLayer = HHSplash.create()
	Engine.get_main_loop().root.add_child(splash)
	for _frame in 39:
		splash.advance(0.05)
	check(not splash.done, "still showing at 1.95 s")
	splash.advance(0.05)
	check(splash.done, "done at 2.0 s, not slowed down")
	return done()


func test_a_long_first_frame_neither_skips_the_fade_nor_the_splash() -> bool:
	var splash: CanvasLayer = HHSplash.create()
	Engine.get_main_loop().root.add_child(splash)
	splash.advance(2.2)
	check(not splash.done, "a 2.2 s hitch must not end the splash")
	check(splash.frame.get_node("Content").modulate.a < 0.2, "and the fade in has barely started")
	splash.free()
	return done()


func test_any_press_skips_but_a_release_repeat_wheel_or_motion_does_not() -> bool:
	var key := InputEventKey.new()
	key.pressed = true
	check(HHSplash.skips(key), "a key press")
	key.echo = true
	check(not HHSplash.skips(key), "a held key's repeat")
	var click := InputEventMouseButton.new()
	click.button_index = MOUSE_BUTTON_LEFT
	click.pressed = false
	check(not HHSplash.skips(click), "a mouse release")
	click.pressed = true
	check(HHSplash.skips(click), "a click")
	var wheel := InputEventMouseButton.new()
	wheel.button_index = MOUSE_BUTTON_WHEEL_DOWN
	wheel.pressed = true
	check(not HHSplash.skips(wheel), "a wheel scroll")
	var touch := InputEventScreenTouch.new()
	touch.pressed = true
	check(HHSplash.skips(touch), "a touch")
	var pad := InputEventJoypadButton.new()
	pad.pressed = true
	check(HHSplash.skips(pad), "a pad button")
	check(not HHSplash.skips(InputEventMouseMotion.new()), "moving the mouse")
	return done()


func test_it_fills_the_screen_under_a_2d_scene_and_draws_above_game_layers() -> bool:
	var world := Node2D.new()
	Engine.get_main_loop().root.add_child(world)
	var splash: CanvasLayer = HHSplash.create()
	world.add_child(splash)
	var screen: Vector2 = splash.get_viewport().get_visible_rect().size
	check(screen.x > 0 and screen.y > 0, "the test viewport has a size")
	check_eq(splash.frame.size, screen, "the splash covers the viewport")
	check(splash.layer > 100, "above ordinary game canvas layers")
	check_eq(splash.frame.get_node("Field").color, Tokens.BG)
	check_eq(splash.frame.get_node("Content/Wordmark").text, "HOOTON HEROICS")
	world.free()
	return done()


func test_landscape_puts_the_host_beside_the_mark_with_clear_space() -> bool:
	var rects := HHSplash.layout_for(Vector2(1280, 720), true)
	var mark: Rect2 = rects["mark"]
	var host: Rect2 = rects["host"]
	check(rects["beside"], "wide enough for the host beside the mark")
	check(is_equal_approx(mark.size.y, 720 * Tokens.SPLASH_LOGO_HEIGHT), "mark height follows the token")
	check(mark.position.x - host.end.x >= mark.size.y / 4.0 - 0.01, "a quarter of the mark between host and mark")
	check(host.position.x >= 0, "host on screen")
	check(rects["wordmark"].position.y - mark.end.y >= mark.size.y / 4.0 - 0.01, "clear space above the wordmark")
	return done()


func test_portrait_and_square_screens_stack_the_host_above_the_mark_on_screen() -> bool:
	for screen in [Vector2(720, 1280), Vector2(1000, 1000), Vector2(390, 844)]:
		var rects := HHSplash.layout_for(screen, true)
		var host: Rect2 = rects["host"]
		var mark: Rect2 = rects["mark"]
		check(not rects["beside"], "%s stacks" % screen)
		check(Rect2(Vector2.ZERO, screen).encloses(host), "%s host on screen: %s" % [screen, host])
		check(mark.position.y - host.end.y >= mark.size.y / 4.0 - 0.01, "%s clear space under the host" % screen)
		check(rects["wordmark"].end.y <= screen.y, "%s wordmark on screen" % screen)
	var bare := HHSplash.layout_for(Vector2(720, 1280), false)
	check_eq(bare["mark"].get_center(), Vector2(360, 640), "no host: the mark is centred")
	return done()
