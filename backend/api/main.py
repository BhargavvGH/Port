from fastapi import FastAPI
import pandas as pd
from pathlib import Path

app = FastAPI(title="Jebel Ali Port Digital Twin API")

BASE_DIR = Path(__file__).resolve().parent.parent.parent
DATA_PATH = BASE_DIR / "data" / "simulated"

@app.get("/")
def root():
    return {"status": "Digital Twin API running"}

@app.get("/ships")
def get_ships():
    file_path = DATA_PATH / "ships.csv"

    if not file_path.exists():
        return {
            "error": "ships.csv not found",
            "expected_path": str(file_path)
        }

    ships = pd.read_csv(file_path)

    return ships.to_dict(orient="records")

@app.get("/yard")
def get_yard():
    return pd.read_csv(DATA_PATH + "yard_occupancy.csv").to_dict(orient="records")

@app.get("/yard/status")
def yard_status():
    file_path = DATA_PATH / "yard_occupancy.csv"

    if not file_path.exists():
        return {
            "error": "yard_occupancy.csv not found",
            "expected_path": str(file_path)
        }

    yard = pd.read_csv(file_path)

    if yard.empty:
        return {
            "occupancy": 0,
            "alert": False
        }

    latest = yard.iloc[-1]
    occupancy = int(latest["occupancy_percent"])

    return {
        "occupancy": occupancy,
        "alert": bool(occupancy > 85)
    }

from ..models.congestion_model import predict_congestion

@app.get("/ai/congestion")
def congestion_prediction():
    prediction = predict_congestion()

    return {
        "predicted_occupancy": round(prediction, 2),
        "risk": bool(prediction > 85)
    }

