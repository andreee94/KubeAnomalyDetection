import unittest
import os

from models.settings.settings_storage import SettingsStorage

class TestSettingsStorage(unittest.TestCase):

    def setUp(self) -> None:
        return super().setUp()

    def test_load_from_env_with_valid_values_should_succeed(self):  
        os.environ["STORAGE_HOST"] = "STORAGE_HOST"
        os.environ["STORAGE_TYPE"] = "STORAGE_TYPE"
        os.environ["STORAGE_USERNAME"] = "STORAGE_USERNAME"
        os.environ["STORAGE_PASSWORD"] = "STORAGE_PASSWORD"
        os.environ["STORAGE_CONTAINER"] = "STORAGE_CONTAINER"

        settings = SettingsStorage.load_from_env()

        self.assertIsNotNone(settings)
        self.assertEqual(settings.storage_host, "STORAGE_HOST")
        self.assertEqual(settings.storage_type, "STORAGE_TYPE")
        self.assertEqual(settings.storage_username, "STORAGE_USERNAME")
        self.assertEqual(settings.storage_password, "STORAGE_PASSWORD")
        self.assertEqual(settings.storage_container, "STORAGE_CONTAINER")
        
    
    def test_load_from_env_with_missing_values_should_fail(self):  
        os.unsetenv("STORAGE_HOST")

        with self.assertRaises(Exception) as context:
            _ = SettingsStorage.load_from_env()


if __name__ == '__main__':
    unittest.main()
