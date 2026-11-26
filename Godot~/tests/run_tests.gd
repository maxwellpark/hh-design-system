extends SceneTree
## Headless test runner: godot --headless --path . -s res://tests/run_tests.gd
##
## Godot keeps running after a GDScript runtime error and still exits 0, so a test only passes when
## its method returns true, which it can only do by reaching its last line with no failed check.

func _initialize() -> void:
	# Nodes added before the first frame wait for it to run _ready, so tests start on that frame.
	process_frame.connect(_run, CONNECT_ONE_SHOT)


func _run() -> void:
	var names := Array(DirAccess.get_files_at("res://tests")).filter(
		func(file): return file.begins_with("test_") and file.ends_with(".gd"))
	names.sort()
	var total := 0
	var failed := 0
	for file in names:
		var script: GDScript = load("res://tests/" + file)
		if script == null or not script.can_instantiate():
			failed += 1
			print("FAIL %s did not load" % file)
			continue
		for method in script.get_script_method_list():
			var test: String = method["name"]
			if not test.begins_with("test_"):
				continue
			total += 1
			var case: RefCounted = script.new()
			var passed = case.call(test)
			if passed == true:
				print("ok   %s %s" % [file, test])
			else:
				failed += 1
				print("FAIL %s %s" % [file, test])
				for line in case.failures:
					print("       " + line)
				if case.failures.is_empty():
					print("       stopped before its end (script error above)")
	print("%d tests, %d failed" % [total, failed])
	quit(1 if failed > 0 or total == 0 else 0)
