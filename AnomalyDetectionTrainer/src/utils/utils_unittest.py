# from test.extended_test_case import ExtendedTestCase
from utils import utils
import unittest
import os 
from parameterized import parameterized


class TestUtils(unittest.TestCase):

    def setUp(self) -> None:
        return super().setUp()

#########################################################################

    def test_raise_if_none_with_none_should_raise(self):
        with self.assertRaises(Exception) as context:
            utils.raise_if_none(None)


    def test_raise_if_none_with_not_none_should_succeed(self):
        utils.raise_if_none("not_none_value")

#########################################################################

    def test_getenv_or_raise_with_none_should_raise(self):
        var_name = "RANDOM_LONG_ENV"
        os.unsetenv(var_name) # be sure that the variable does not exist

        with self.assertRaises(Exception) as context:
            utils.getenv_or_raise(var_name)


    def test_getenv_or_raise_with_invalid_conversion_should_raise(self):
        var_name = "TEST_ENV"
        var_value = "2.2222"
        os.environ[var_name] = var_value

        with self.assertRaises(Exception) as context:
            utils.getenv_or_raise(var_name, output_type=float)


    def test_getenv_or_raise_with_bool_conversion_should_succeed(self):
        var_name = "TEST_ENV"
        var_value = "false"
        os.environ[var_name] = var_value

        result = utils.getenv_or_raise(var_name, output_type=bool)

        self.assertFalse(result)

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
