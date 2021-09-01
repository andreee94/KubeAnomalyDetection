import unittest
import os

from models.settings import Settings 


class TestSettings(unittest.TestCase):

    def setUp(self) -> None:
        return super().setUp()

    def test_load_from_env_with_valid_values_not_autenticated_should_succeed(self):  

        os.environ["QUERY"] = "example_query"
        os.environ["DATASOURCE_URL"] = "https://example.url.com"
        os.environ["DATASOURCE_TYPE"] = "example_datasource_type"
        os.environ["DATASOURCE_IS_AUTHENTICATED"] = "False"
        os.environ["DATASOURCE_USERNAME"] = "username"
        os.environ["DATASOURCE_PASSWORD"] = "password"

        settings = Settings.load_from_env()

        self.assertIsNotNone(settings)
        self.assertEqual(settings.query, "example_query")
        self.assertEqual(settings.datasource_url, "https://example.url.com")
        self.assertEqual(settings.datasource_type, "example_datasource_type")
        self.assertEqual(settings.datasource_is_authenticated, False)
        self.assertEqual(settings.datasource_username, None)
        self.assertEqual(settings.datasource_password, None)
        
    
    def test_load_from_env_with_valid_values_autenticated_should_succeed(self):  

        os.environ["QUERY"] = "example_query"
        os.environ["DATASOURCE_URL"] = "https://example.url.com"
        os.environ["DATASOURCE_TYPE"] = "example_datasource_type"
        os.environ["DATASOURCE_IS_AUTHENTICATED"] = "True"
        os.environ["DATASOURCE_USERNAME"] = "username"
        os.environ["DATASOURCE_PASSWORD"] = "password"

        settings = Settings.load_from_env()

        self.assertIsNotNone(settings)
        self.assertEqual(settings.query, "example_query")
        self.assertEqual(settings.datasource_url, "https://example.url.com")
        self.assertEqual(settings.datasource_type, "example_datasource_type")
        self.assertEqual(settings.datasource_is_authenticated, True)
        self.assertEqual(settings.datasource_username, "username")
        self.assertEqual(settings.datasource_password, "password")

    # def test_get_datasource_implementation_with_valid_type_should_work(self):
    #     url = "https://test.url.com"
    #     is_authenticated = False
    #     username = "username"
    #     datasource_type = PrometheusDatasource.TYPE

    #     result = Datasources.get_datasource_implementation(url, datasource_type, is_authenticated=is_authenticated, username=username)

    #     self.assertIsNotNone(result)
    #     self.assertIsInstance(result, PrometheusDatasource)
    #     self.assertEqual(result.url, url)
    #     self.assertEqual(result.is_authenticated, is_authenticated)
    #     self.assertEqual(result.datasource_type, datasource_type)
    #     self.assertEqual(result.username, username)
    #     self.assertIsNone(result.password)
    

    # def test_get_datasource_implementation_with_invalid_type_should_return_none(self):
    #     url = "https://test.url.com"
    #     is_authenticated = False
    #     datasource_type = "RANDOM_TYPE_THAT_DOES_NOT_EXIST"

    #     result = Datasources.get_datasource_implementation(url, datasource_type, is_authenticated=is_authenticated)

    #     self.assertIsNone(result)
    

if __name__ == '__main__':
    unittest.main()
