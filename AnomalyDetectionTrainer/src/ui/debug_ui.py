import streamlit as st
import matplotlib.pyplot as plt
import pandas as pd

class DebugUI:


    def __init__(self):
        pass


    def setup(self):
        st.title("Debug UI - Training Results")


    def show_dataframe_as_plot(self, df: pd.DataFrame):
        fig = plt.figure()
        df = df.set_index('ds')

        print(df.columns)

        print(df.head())

        plt.plot(df.yhat, ".")
        # st.line_chart(df)
        st.plotly_chart(fig)


    def show_dataframe_as_table(self, df):
        st.table(df)