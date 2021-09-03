from dataclasses import dataclass
import os
from utils import utils


@dataclass
class SettingsForecast:
    forecast_window: int = 3600 * 6 # 6h
    forecast_step: int = 1800 # 30 min


    @property
    def forecast_period(self) -> int:
        return int(self.forecast_window / self.forecast_step)
        
    @property
    def forecast_step_literal(self) -> int:
        return f"{self.forecast_step}S"


    @staticmethod
    def load_from_env():
        forecast_window = utils.getenv_or_raise("FORECAST_WIND0W", output_type=int)
        forecast_step = utils.getenv_or_raise("FORECAST_STEP", output_type=int)

        return SettingsForecast(
            forecast_window=forecast_window,
            forecast_step=forecast_step
        )