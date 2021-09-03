import unittest
import os

from models.settings.settings_retraining import SettingsRetraining

class TestSettingsRetraining(unittest.TestCase):

    def setUp(self) -> None:
        return super().setUp()

    def test_load_from_env_with_valid_values_should_succeed(self):  
        os.environ["RETRAINING_WINDOW_SIZE"] = "86400"
        os.environ["RETRAINING_INTERVAL"] = "7200"

        settings = SettingsRetraining.load_from_env()

        self.assertIsNotNone(settings)
        self.assertEqual(settings.retraining_window_size, 86400)
        self.assertEqual(settings.retraining_interval, 7200)
        
    
    def test_load_from_env_with_invalid_values_should_fail(self):  
        os.environ["RETRAINING_WINDOW_SIZE"] = "fail"
        os.environ["RETRAINING_INTERVAL"] = "7200"

        with self.assertRaises(ValueError) as context:
            _ = SettingsRetraining.load_from_env()


    def test_load_from_env_with_missing_values_should_fail(self):  
        os.unsetenv("RETRAINING_INTERVAL")

        with self.assertRaises(Exception) as context:
            _ = SettingsRetraining.load_from_env()


if __name__ == '__main__':
    unittest.main()
