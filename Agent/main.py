from flask import Flask
from flask_sock import Sock
from pipeline import Agent
import json
import logging

app = Flask(__name__)
sock = Sock(app)
agent = Agent()
logger = logging.getLogger(__name__)

@sock.route('/agent')
def agent_websocket(ws):
    while True:
        try:
            # Receive game state from C#
            game_state = json.loads(ws.receive())
            
            # Process and send action back
            action = agent.process(game_state)
            ws.send(json.dumps(action))
        except Exception as e:
            logger.error(f"Error: {e}")
            break

if __name__ == '__main__':
    app.run(host='127.0.0.1', port=5000)