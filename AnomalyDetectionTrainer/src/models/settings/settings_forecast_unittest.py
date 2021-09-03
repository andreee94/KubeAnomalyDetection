import unittest
import os

from models.settings.settings_forecast import SettingsForecast

class TestSettingsForecast(unittest.TestCase):

    def setUp(self) -> None:
        return super().setUp()

    def test_load_from_env_with_valid_values_should_succeed(self):  
        os.environ["FORECAST_WIND0W"] = "21600"
        os.environ["FORECAST_STEP"] = "1800"

        settings = SettingsForecast.load_from_env()

        self.assertIsNotNone(settings)
        self.assertEqual(settings.forecast_window, 21600)
        self.assertEqual(settings.forecast_step, 1800)
        self.assertEqual(settings.forecast_period, 12)
        
    
    def test_load_from_env_with_invalid_values_should_fail(self):  
        os.environ["FORECAST_WIND0W"] = "fail"
        os.environ["FORECAST_STEP"] = "1800"

        with self.assertRaises(ValueError) as context:
            _ = SettingsForecast.load_from_env()


    def test_load_from_env_with_missing_values_should_fail(self):  
        os.unsetenv("FORECAST_STEP")

        with self.assertRaises(Exception) as context:
            _ = SettingsForecast.load_from_env()


if __name__ == '__main__':
    unittest.main()
