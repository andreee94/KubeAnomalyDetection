import unittest
import os

from models.settings.settings_datasource import SettingsDatasource

class TestSettingsDatasource(unittest.TestCase):

    def setUp(self) -> None:
        return super().setUp()

    def test_load_from_env_with_valid_values_not_autenticated_should_succeed(self):  
        os.environ["DATASOURCE_URL"] = "https://example.url.com"
        os.environ["DATASOURCE_TYPE"] = "example_datasource_type"
        os.environ["DATASOURCE_IS_AUTHENTICATED"] = "False"
        os.environ["DATASOURCE_USERNAME"] = "username"
        os.environ["DATASOURCE_PASSWORD"] = "password"

        settings = SettingsDatasource.load_from_env()

        self.assertIsNotNone(settings)
        self.assertEqual(settings.datasource_url, "https://example.url.com")
        self.assertEqual(settings.datasource_type, "example_datasource_type")
        self.assertFalse(settings.datasource_is_authenticated)
        self.assertIsNone(settings.datasource_username)
        self.assertIsNone(settings.datasource_password)
        

    def test_load_from_env_with_valid_values_autenticated_should_succeed(self):  
        os.environ["DATASOURCE_URL"] = "https://example.url.com"
        os.environ["DATASOURCE_TYPE"] = "example_datasource_type"
        os.environ["DATASOURCE_IS_AUTHENTICATED"] = "True"
        os.environ["DATASOURCE_USERNAME"] = "username"
        os.environ["DATASOURCE_PASSWORD"] = "password"

        settings = SettingsDatasource.load_from_env()

        self.assertIsNotNone(settings)
        self.assertEqual(settings.datasource_url, "https://example.url.com")
        self.assertEqual(settings.datasource_type, "example_datasource_type")
        self.assertTrue(settings.datasource_is_authenticated)
        self.assertEqual(settings.datasource_username, "username")
        self.assertEqual(settings.datasource_password, "password")
        

    def test_load_from_env_with_missing_values_should_fail(self):  
        os.unsetenv("DATASOURCE_URL")

        with self.assertRaises(Exception) as context:
            _ = SettingsDatasource.load_from_env()


if __name__ == '__main__':
    unittest.main()
