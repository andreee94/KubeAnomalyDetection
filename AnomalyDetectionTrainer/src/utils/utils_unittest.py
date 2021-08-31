# from test.extended_test_case import ExtendedTestCase
from utils import utils
import unittest
import os 


class TestUtils(unittest.TestCase):

    def setUp(self) -> None:
        return super().setUp()


    def test_raise_if_none_with_none_should_raise(self):
        with self.assertRaises(Exception) as context:
            utils.raise_if_none(None)


    def test_raise_if_none_with_not_none_should_success(self):
        utils.raise_if_none("not_none_value")


    def test_getenv_or_raise_with_none_should_raise(self):
        var_name = "RANDOM_LONG_ENV"
        os.unsetenv(var_name) # be sure that the variable does not exist

        with self.assertRaises(Exception) as context:
            utils.getenv_or_raise(var_name)



if __name__ == '__main__':
    unittest.main()
