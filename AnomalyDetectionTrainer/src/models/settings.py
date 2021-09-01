from dataclasses import dataclass
import os
from utils import utils


@dataclass
class Settings:
    query: str

    datasource_url: str
    datasource_type: str
    datasource_is_authenticated: bool
    datasource_username: str = None
    datasource_password: str = None

    @staticmethod
    def load_from_env():
        query = utils.getenv_or_raise("QUERY")
        datasource_url = utils.getenv_or_raise("DATASOURCE_URL")
        datasource_type = utils.getenv_or_raise("DATASOURCE_TYPE")
        datasource_is_authenticated = utils.getenv_or_raise(
            "DATASOURCE_IS_AUTHENTICATED", bool)

        if datasource_is_authenticated:
            datasource_username = utils.getenv_or_raise("DATASOURCE_USERNAME")
            datasource_password = utils.getenv_or_raise("DATASOURCE_PASSWORD")
        else:
            datasource_username = datasource_password = None

        settings = Settings(query,
                            datasource_url,
                            datasource_type,
                            datasource_is_authenticated=datasource_is_authenticated,
                            datasource_username=datasource_username,
                            datasource_password=datasource_password
                            )

        return settings