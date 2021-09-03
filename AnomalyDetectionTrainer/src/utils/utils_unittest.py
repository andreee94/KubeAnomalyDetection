# from test.extended_test_case import ExtendedTestCase
from utils import utils
import unittest
import os
from parameterized import parameterized
import uuid


class TestUtils(unittest.TestCase):

    def setUp(self) -> None:
        return super().setUp()

    def get_random_var_name(self):
        return str(uuid.uuid1()).replace('-', '')

#########################################################################

    def test_raise_if_none_with_none_should_raise(self):
        with self.assertRaises(Exception) as context:
            utils.raise_if_none(None)

    def test_raise_if_none_with_not_none_should_succeed(self):
        utils.raise_if_none("not_none_value")

#########################################################################

    def test_getenv_with_bool_should_succeed(self):
        var_name = self.get_random_var_name()
        os.environ[var_name] = "true"

        result = utils.getenv(var_name, default=False, output_type=bool)

        self.assertTrue(result)

    def test_getenv_with_int_should_succeed(self):
        var_name = self.get_random_var_name()
        os.environ[var_name] = "1000"

        result = utils.getenv(var_name, default=0, output_type=int)

        self.assertEqual(result, 1000)

    def test_getenv_with_int_should_succeed(self):
        var_name = self.get_random_var_name()
        os.environ[var_name] = "1000"

        result = utils.getenv(var_name, default=0, output_type=int)

        self.assertEqual(result, 1000)

    def test_getenv_with_str_should_succeed(self):
        var_name = self.get_random_var_name()
        os.environ[var_name] = "text"

        result = utils.getenv(var_name, default=None)

        self.assertEqual(result, "text")

    def test_getenv_with_none_should_return_default(self):
        var_name = self.get_random_var_name()
        os.unsetenv(var_name)  # be sure that the variable does not exist

        result = utils.getenv(var_name, default="long_default_text")

        self.assertEqual(result, "long_default_text")

    def test_getenv_with_float_should_raise_not_implemented(self):
        var_name = self.get_random_var_name()
        os.environ[var_name] = "text"

        with self.assertRaises(NotImplementedError) as context:
            result = utils.getenv(var_name, output_type=float)




#########################################################################

    def test_getenv_or_raise_with_none_should_raise(self):
        var_name = self.get_random_var_name()
        os.unsetenv(var_name)  # be sure that the variable does not exist

        with self.assertRaises(Exception) as context:
            utils.getenv_or_raise(var_name)

    def test_getenv_or_raise_with_invalid_conversion_should_raise(self):
        var_name = self.get_random_var_name()
        var_value = "2.2222"
        os.environ[var_name] = var_value

        with self.assertRaises(Exception) as context:
            utils.getenv_or_raise(var_name, output_type=float)

    def test_getenv_or_raise_with_bool_conversion_should_succeed(self):
        var_name = self.get_random_var_name()
        var_value = "false"
        os.environ[var_name] = var_value

        result = utils.getenv_or_raise(var_name, output_type=bool)

        self.assertFalse(result)

    def test_getenv_or_raise_with_int_conversion_should_succeed(self):
        var_name = self.get_random_var_name()
        var_value = "1000"
        os.environ[var_name] = var_value

        result = utils.getenv_or_raise(var_name, output_type=int)

        self.assertEqual(result, 1000)

    def test_getenv_or_raise_with_invalid_int_conversion_should_fail(self):
        var_name = self.get_random_var_name()
        var_value = "fail"
        os.environ[var_name] = var_value

        with self.assertRaises(ValueError) as context:
            _ = utils.getenv_or_raise(var_name, output_type=int)


#########################################################################

    @parameterized.expand([
        ("true", True),
        ("True", True),
        ("truE", True),
        ("1", True),
        ("t", True),
        ("y", True),
        ("yeah", True),
        ("yup", True),
        ("0", False),
        ("false", False),
        ("random", False)
    ])
    def test_parse_bool_or_raise_with_valid_strings_should_succeed(self, input: str, expected: bool):
        self.assertEqual(utils.parse_bool_or_raise(input), expected)

    @parameterized.expand([
        (0, False),
        (1, True),
        (2, True),
        (True, True),
        (False, False)
    ])
    def test_parse_bool_or_raise_with_valid_ints_should_succeed(self, input: int, expected: bool):
        self.assertEqual(utils.parse_bool_or_raise(input), expected)

    @parameterized.expand([
        (0.01, ),
        (-100., ),
        (None, )
    ])
    def test_parse_bool_or_raise_with_invalid_values_should_fail(self, input):
        with self.assertRaises(Exception) as context:
            utils.parse_bool_or_raise(input)

#########################################################################


if __name__ == '__main__':
    unittest.main()
