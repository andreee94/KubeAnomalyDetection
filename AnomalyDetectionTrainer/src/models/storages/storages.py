from models.storages.postgres_storage import PostgresStorage
from models.settings.settings_storage import SettingsStorage

class Storages:

    STORAGES_DICT = {PostgresStorage.TYPE: PostgresStorage}

    @staticmethod
    def get_storage_implementation(host: str, storage_type: str, username: str, password: str, container: str, **kwargs):

        storage_class = Storages.STORAGES_DICT.get(storage_type, None)

        if storage_class is None:
            print(f"Storage Type: {storage_type} is invalid. Valid values are: {list(Storages.STORAGES_DICT.keys())}")
            return None

        return storage_class(
            host=host,
            storage_type=storage_type,
            username=username,
            password=password,
            container=container,
            **kwargs    
        )

    @staticmethod
    def get_storage_implementation_from_settings(settings: SettingsStorage):
        return Storages.get_storage_implementation(
            host=settings.storage_host,
            storage_type=settings.storage_type,
            username=settings.storage_username,
            password=settings.storage_password,
            container=settings.storage_container
        )
