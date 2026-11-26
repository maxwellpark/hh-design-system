extends RefCounted
## Base for test files. A test method ends with `return done()`.

var failures: PackedStringArray = []


func check(condition: bool, message: String) -> void:
	if not condition:
		failures.append(message)


func check_eq(actual, expected, message := "") -> void:
	if typeof(actual) != typeof(expected) or actual != expected:
		failures.append("%s expected %s, got %s" % [message, var_to_str(expected), var_to_str(actual)])


func done() -> bool:
	return failures.is_empty()
