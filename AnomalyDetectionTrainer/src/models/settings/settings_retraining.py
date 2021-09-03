from dataclasses import dataclass
from utils import utils


@dataclass
class SettingsRetraining:
    retraining_window_size: int = 3600 * 24 * 7  # 7d
    retraining_interval: int = 3600 * 2  # 2h


    @staticmethod
    def load_from_env():

        retraining_window_size = utils.getenv_or_raise("RETRAINING_WINDOW_SIZE", output_type=int)
        retraining_interval = utils.getenv_or_raise("RETRAINING_INTERVAL", output_type=int)

        return SettingsRetraining(
            retraining_window_size=retraining_window_size,
            retraining_interval=retraining_interval
        )