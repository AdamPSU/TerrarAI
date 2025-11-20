from langchain.agents import create_agent
from langchain.agents.structured_output import ToolStrategy
from langchain.tools import tool 
from dataclasses import dataclass, asdict
from typing import List
from dotenv import load_dotenv; load_dotenv()

@dataclass
class ActionCommand:
    tool: str
    parameters: dict

@dataclass
class ResponseFormat:
    log: str
    commands: List[ActionCommand]

class Agent:
    def __init__(self):
        with open("Source/prompt.txt", "r") as file:
            prompt = file.read()

        self.agent = create_agent(
            model="grok-4-fast",
            tools=[hold_item, mine_blocks, kill_enemy, swing_weapon, move],
            system_prompt=prompt,
            response_format=ToolStrategy(ResponseFormat)
        )   

    def process(self, game_state: dict) -> dict:
        result = self.agent.invoke({
            "messages": [{"role": "user", "content": str(game_state)}]
        })
        
        structured = result.get("structured_response")
        if structured:
            return asdict(structured)
        return result

@tool
def hold_item(item_name: str) -> str:
    """Equips an item from inventory. Item name should match inventory item exactly."""
    return f"Equipping {item_name}"

@tool
def mine_blocks(target: str) -> str:
    """Destroys blocks or walls at the specified target. Target can be direction (down/left/right/up) or 'continuous'."""
    return f"Mining blocks: {target}"

@tool
def kill_enemy(target: str) -> str:
    """Attacks and kills the specified enemy entity. Target should be enemy name or identifier."""
    return f"Attacking enemy: {target}"

@tool
def move(direction: str, distance: float = 1.0) -> str:
    """Moves the player in the specified direction. Direction can be left/right/up/down/target."""
    return f"Moving {direction} for distance {distance}"

@tool
def swing_weapon() -> str:
    """Swing the currently held weapon to attack enemies or break objects."""
    return "Swinging weapon"



