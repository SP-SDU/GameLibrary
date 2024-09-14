import subprocess
import os

subprocess.run(["pip", "install", "pipreqs"], check=True)
subprocess.run(["pipreqs", os.path.dirname(os.path.abspath(__file__)), "--force", "--ignore", "env"], check=True) # noqa