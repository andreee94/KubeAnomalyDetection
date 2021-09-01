from typing import List
from models.datasources.prometheus_datasource import PrometheusDatasource
from models.settings import Settings

class Datasources:

    DATASOURCES_DICT = {PrometheusDatasource.TYPE: PrometheusDatasource}

    @staticmethod
    def get_datasource_implementation(url: str, datasource_type: str, is_authenticated: bool = False, username: str = None, password: str = None, **kwargs):

        datasource_class = Datasources.DATASOURCES_DICT.get(datasource_type, None)

        if datasource_class is None:
            print(
                f"Datasource Type: {datasource_type} is invalid. Valid values are: {list(Datasources.DATASOURCES_DICT.keys())}")
            return None

        return datasource_class(url, is_authenticated=is_authenticated, username=username, password=password, **kwargs)

    @staticmethod
    def get_datasource_implementation_from_settings(settings: Settings):
        return Datasources.get_datasource_implementation(settings.datasource_url, settings.datasource_type, is_authenticated=settings.datasource_is_authenticated, username=settings.datasource_username, password=settings.datasource_password)
