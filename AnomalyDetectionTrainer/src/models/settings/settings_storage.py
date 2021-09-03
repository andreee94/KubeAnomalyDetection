from dataclasses import dataclass
import os
from utils import utils


@dataclass
class SettingsStorage:
    storage_host: str
    storage_type: str
    storage_username: str
    storage_password: str
    storage_container: str

    @staticmethod
    def load_from_env():
        storage_host = utils.getenv_or_raise("STORAGE_HOST")
        storage_type = utils.getenv_or_raise("STORAGE_TYPE")
        storage_username = utils.getenv_or_raise("STORAGE_USERNAME")
        storage_password = utils.getenv_or_raise("STORAGE_PASSWORD")
        storage_container = utils.getenv_or_raise("STORAGE_CONTAINER")

        return SettingsStorage(
            storage_host=storage_host,
            storage_type=storage_type,
            storage_username=storage_username,
            storage_password=storage_password,
            storage_container=storage_container
        )