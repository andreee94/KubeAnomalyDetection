import unittest
import os
import time

from models.settings.settings import Settings 


class TestSettings(unittest.TestCase):

    def setUp(self) -> None:
        return super().setUp()

    def test_load_from_env_with_missing_values_should_fail(self):  
        os.unsetenv("QUERY")

        with self.assertRaises(Exception) as context:
            _ = Settings.load_from_env()

    def test_load_from_env_with_valid_values_should_succeed(self):  
        os.environ["QUERY"] = "example_query"
        os.environ["DEBUG"] = "False"

        os.environ["DATASOURCE_URL"] = "https://example.url.com"
        os.environ["DATASOURCE_TYPE"] = "example_datasource_type"
        os.environ["DATASOURCE_IS_AUTHENTICATED"] = "False"

        os.environ["RETRAINING_WINDOW_SIZE"] = "86400"
        os.environ["RETRAINING_INTERVAL"] = "7200"

        os.environ["FORECAST_WIND0W"] = "21600"
        os.environ["FORECAST_STEP"] = "1800"

        settings = Settings.load_from_env()

        self.assertIsNotNone(settings)
        self.assertEqual(settings.query, "example_query")
        self.assertEqual(settings.datasource.datasource_url, "https://example.url.com")
        self.assertEqual(settings.datasource.datasource_type, "example_datasource_type")
        self.assertEqual(settings.datasource.datasource_is_authenticated, False)
        self.assertEqual(settings.datasource.datasource_username, None)
        self.assertEqual(settings.datasource.datasource_password, None)
        self.assertEqual(settings.retraining.retraining_window_size, 86400)
        self.assertEqual(settings.retraining.retraining_interval, 7200)
        self.assertEqual(settings.forecast.forecast_window, 21600)
        self.assertEqual(settings.forecast.forecast_step, 1800)
        

if __name__ == '__main__':
    unittest.main()
