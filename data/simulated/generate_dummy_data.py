import pandas as pd
import numpy as np
from datetime import datetime, timedelta
import random

np.random.seed(42)


# Ships Data
ships = []
start_time = datetime(2026, 1, 1)

for i in range(20):
    arrival = start_time + timedelta(hours=i * np.random.randint(1, 3))
    handling_hours = random.randint(8, 16)
    departure = arrival + timedelta(hours=handling_hours)
    ships.append({
        'ship_id': f'S{i+101}',
        'arrival_time': arrival,
        'departure_time': departure,
        'berth_id': f"B{random.randint(1,5)}",
        'container_count': random.randint(50, 2000)
    })

ships_df = pd.DataFrame(ships)
ships_df.to_csv("ships.csv", index=False)


# Cranes Data
cranes = []
for i in range(5):
    cranes.append({
        "crane_id": f"C{i+1}",
        "status": random.choice(["idle", "operating"]),
        "containers_per_hour": random.randint(25, 40)
    })

cranes_df = pd.DataFrame(cranes)
cranes_df.to_csv("cranes.csv", index=False)


# Containers Data
containers = []
for i in range(3000):
    containers.append({
        "container_id": f"CONT{i+1}",
        "yard_block": random.choice(["A", "B", "C"]),
        "slot": random.randint(1, 100),
        "status": random.choice(["stored", "in_transit"])
    })

containers_df = pd.DataFrame(containers)
containers_df.to_csv("containers.csv", index=False)


#Yard Occuppancy (Time Series Data)
yard = []
occupancy = 60

for i in range(24):
    occupancy += random.randint(-2, 6)
    occupancy = max(50, min(95, occupancy))

    yard.append({
        "timestamp": start_time + timedelta(hours=i),
        "occupancy_percent": occupancy
    })


yard_df = pd.DataFrame(yard)
yard_df.to_csv("yard_occupancy.csv", index=False)


print("Dummy data generated successfully.") 