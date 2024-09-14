import subprocess
import sys
import os
import webbrowser
import threading

# Define constants
SCRIPT_PATH = os.path.dirname(os.path.abspath(__file__))
VENV_DIR = "env"
ENV_PATH = os.path.join(SCRIPT_PATH, VENV_DIR)
VENV_BIN_PATH = os.path.join(ENV_PATH, "Scripts" if os.name == 'nt' else "bin")
PYTHON_PATH = os.path.join(VENV_BIN_PATH, "python")
PIP_PATH = os.path.join(VENV_BIN_PATH, "pip")
PIPREQS_PATH = os.path.join(VENV_BIN_PATH, "pipreqs")
MANAGE_PATH = os.path.join(SCRIPT_PATH, "manage.py")
REQUIREMENTS_PATH = os.path.join(SCRIPT_PATH, "requirements.txt")


def create_virtualenv(env_path: str) -> None:
    """Create a virtual environment if it does not exist."""
    if not os.path.isdir(env_path):
        subprocess.run([sys.executable, "-m", "venv", env_path], check=True)


def update_requirements(script_path: str, pip_path: str, pipreqs_path: str) -> None:  # noqa
    """Update requirements.txt using pipreqs if necessary."""
    if not os.path.isfile(pipreqs_path):
        subprocess.run([pip_path, "install", "pipreqs"], check=True)
    subprocess.run([pipreqs_path, script_path, "--force", "--ignore", VENV_DIR], check=True)  # noqa


def install_requirements(pip_path: str, requirements_path: str) -> None:
    """Install packages from requirements.txt if file exists."""
    if os.path.isfile(requirements_path):
        subprocess.run([pip_path, "install", "-r", requirements_path], check=True)  # noqa


def migrate_database(python_path: str, manage_path: str) -> None:
    """Apply Django database migrations."""
    if os.path.isfile(manage_path):
        subprocess.run([python_path, manage_path, "migrate"], check=True)


def run_django_server(python_path: str, manage_path: str) -> None:
    """Run the Django development server."""
    if os.path.isfile(manage_path):
        try:
            subprocess.run([python_path, manage_path, "runserver"], check=True)
        except KeyboardInterrupt:
            pass


def open_browser() -> None:
    """Open the web browser asynchronously."""
    webbrowser.open("http://127.0.0.1:8000")


if __name__ == "__main__":
    # Create virtual environment
    create_virtualenv(ENV_PATH)

    # Update requirements and install them
    update_requirements(SCRIPT_PATH, PIP_PATH, PIPREQS_PATH)
    install_requirements(PIP_PATH, REQUIREMENTS_PATH)

    # Migrate database and run server
    migrate_database(PYTHON_PATH, MANAGE_PATH)
    threading.Thread(target=open_browser, daemon=True).start()
    run_django_server(PYTHON_PATH, MANAGE_PATH)
