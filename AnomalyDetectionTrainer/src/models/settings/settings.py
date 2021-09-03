from dataclasses import dataclass
from logging import debug
import os

import prophet
from models.settings.settings_datasource import SettingsDatasource
from models.settings.settings_forecast import SettingsForecast
from models.settings.settings_retraining import SettingsRetraining
from models.settings.settings_storage import SettingsStorage
from utils import utils


@dataclass
class Settings:
    query: str
    debug: bool

    datasource: SettingsDatasource
    storage: SettingsStorage
    retraining: SettingsRetraining
    forecast: SettingsForecast


    @staticmethod
    def load_from_env():
        query = utils.getenv_or_raise("QUERY")
        debug = utils.getenv("DEBUG", default=False)

        datasource = SettingsDatasource.load_from_env()
        storage = SettingsStorage.load_from_env()
        retraining = SettingsRetraining.load_from_env()
        forecast = SettingsForecast.load_from_env()

        return Settings(
            query=query,
            debug=debug,
            datasource=datasource,
            storage=storage,
            retraining=retraining,
            forecast=forecast
        )


# @dataclass
# class Settings:
#     query: str

#     datasource_url: str
#     datasource_type: str
#     datasource_is_authenticated: bool
#     datasource_username: str = None
#     datasource_password: str = None

#     retraining_window_size: int = 3600 * 24 * 7 # 7d
#     retraining_interval: int = 3600 * 2 # 2h

#     @staticmethod
#     def load_from_env():
#         query = utils.getenv_or_raise("QUERY")

#         ####################################################

#         datasource_url = utils.getenv_or_raise("DATASOURCE_URL")
#         datasource_type = utils.getenv_or_raise("DATASOURCE_TYPE")
#         datasource_is_authenticated = utils.getenv_or_raise(
#             "DATASOURCE_IS_AUTHENTICATED", bool)

#         if datasource_is_authenticated:
#             datasource_username = utils.getenv_or_raise("DATASOURCE_USERNAME")
#             datasource_password = utils.getenv_or_raise("DATASOURCE_PASSWORD")
#         else:
#             datasource_username = datasource_password = None

#         ####################################################
        
#         retraining_window_size = utils.getenv_or_raise("RETRAINING_WINDOW_SIZE", output_type=int)
#         retraining_interval = utils.getenv_or_raise("RETRAINING_INTERVAL", output_type=int)

#         ####################################################

#         settings = Settings(query,

#                             datasource_url,
#                             datasource_type,
#                             datasource_is_authenticated=datasource_is_authenticated,
#                             datasource_username=datasource_username,
#                             datasource_password=datasource_password,
                            
#                             retraining_window_size=retraining_window_size,
#                             retraining_interval=retraining_interval
#                             )

#         return settings