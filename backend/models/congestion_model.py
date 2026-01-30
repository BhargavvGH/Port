import pandas as pd
from pathlib import Path
from sklearn.linear_model import LinearRegression

BASE_DIR = Path(__file__).resolve().parent.parent.parent
DATA_PATH = BASE_DIR / "data" / "simulated"

def predict_congestion():
    yard = pd.read_csv(DATA_PATH / "yard_occupancy.csv")

    yard["hour"] = range(len(yard))
    X = yard[["hour"]]
    y = yard["occupancy_percent"]

    model = LinearRegression()
    model.fit(X, y)

    next_hour = [[len(yard)]]
    prediction = model.predict(next_hour)[0]

    return float(prediction)
