import os 
import unittest
import unittest_extension.skip_decorators

class TestSkipDecorators(unittest.TestCase):

#########################################################################

    def test_skipIfEnvIsSet_when_env_is_set(self):
        def func(a, b):
            return a + b

        env_name = "RANDOM_ENV_NAME"
        os.environ[env_name] = "exists"

        result = unittest_extension.skip_decorators.skipIfEnvIsSet(env_name, "reason")(func)

        self.assertNotEqual(result, func)


    def test_skipIfEnvIsSet_when_env_is_not_set(self):
        def func(a, b):
            return a + b

        env_name = "RANDOM_ENV_NAME"
        if env_name in os.environ:
            del os.environ[env_name]

        result = unittest_extension.skip_decorators.skipIfEnvIsSet(env_name, "reason")(func)

        self.assertEqual(result, func)
        self.assertEqual(result(2, 3), 5)

#########################################################################

    def test_skipIfEnvIsUnset_when_env_is_set(self):
        def func(a, b):
            return a + b

        env_name = "RANDOM_ENV_NAME"
        os.environ[env_name] = "exists"

        result = unittest_extension.skip_decorators.skipIfEnvIsUnset(env_name, "reason")(func)

        self.assertEqual(result, func)
        self.assertEqual(result(2, 3), 5)


    def test_skipIfEnvIsUnset_when_env_is_not_set(self):
        def func(a, b):
            return a + b

        env_name = "RANDOM_ENV_NAME"
        if env_name in os.environ:
            del os.environ[env_name]

        result = unittest_extension.skip_decorators.skipIfEnvIsUnset(env_name, "reason")(func)

        self.assertNotEqual(result, func)

#########################################################################

if __name__ == '__main__':
    unittest.main()
