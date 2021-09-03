# from test.extended_test_case import ExtendedTestCase
from unittest import result
from models.datasources.datasource import Datasource
from models.datasources.datasources import Datasources
from models.datasources.prometheus_datasource import PrometheusDatasource
from models.settings.settings import Settings
from models.settings.settings_datasource import SettingsDatasource
from utils import utils
import unittest
import os 


class TestDatasources(unittest.TestCase):

    def setUp(self) -> None:
        return super().setUp()

#########################################################################

    def test_get_datasource_implementation_with_valid_type_should_work(self):
        url = "https://test.url.com"
        is_authenticated = False
        username = "username"
        datasource_type = PrometheusDatasource.TYPE

        result = Datasources.get_datasource_implementation(url, datasource_type, is_authenticated=is_authenticated, username=username)

        self.assertIsNotNone(result)
        self.assertIsInstance(result, PrometheusDatasource)
        self.assertEqual(result.url, url)
        self.assertEqual(result.is_authenticated, is_authenticated)
        self.assertEqual(result.datasource_type, datasource_type)
        self.assertEqual(result.username, username)
        self.assertIsNone(result.password)
    

    def test_get_datasource_implementation_with_invalid_type_should_return_none(self):
        url = "https://test.url.com"
        is_authenticated = False
        datasource_type = "RANDOM_TYPE_THAT_DOES_NOT_EXIST"

        result = Datasources.get_datasource_implementation(url, datasource_type, is_authenticated=is_authenticated)

        self.assertIsNone(result)

#########################################################################

    def test_get_datasource_implementation_from_settings_with_valid_type_should_work(self):

        settings = SettingsDatasource(
            datasource_url = "https://test.url.com",
            datasource_is_authenticated = False,
            datasource_username = "username",
            datasource_type = PrometheusDatasource.TYPE
        )

        result = Datasources.get_datasource_implementation_from_settings(settings)

        self.assertIsNotNone(result)
        self.assertIsInstance(result, PrometheusDatasource)
        self.assertEqual(result.url, settings.datasource_url)
        self.assertEqual(result.is_authenticated, settings.datasource_is_authenticated)
        self.assertEqual(result.datasource_type, settings.datasource_type)
        self.assertEqual(result.username, settings.datasource_username)
        self.assertIsNone(result.password)


    def test_get_datasource_implementation_from_settings_with_invalid_type_should_return_none(self):
        settings = SettingsDatasource(
            datasource_url = "https://test.url.com",
            datasource_is_authenticated = False,
            datasource_type = "RANDOM_TYPE_THAT_DOES_NOT_EXIST"
        )

        result = Datasources.get_datasource_implementation_from_settings(settings)

        self.assertIsNone(result)
    
#########################################################################

if __name__ == '__main__':
    unittest.main()
