import subprocess
import sys
import os
import webbrowser
import threading


def install_requirements(script_dir: str) -> None:
    """Install packages from requirements.txt."""
    try:
        requirements_path = os.path.join(script_dir, "requirements.txt")  # noqa
        subprocess.check_call([sys.executable, "-m", "pip", "install", "-r", requirements_path])  # noqa
    except (FileNotFoundError, subprocess.CalledProcessError) as e:  # noqa
        sys.exit(f"Failed to install packages or requirements.txt not found. Error: {e}")  # noqa


def migrate_database(script_dir: str) -> None:
    """Apply Django database migrations."""
    try:
        os.chdir(os.path.join(script_dir))
        subprocess.check_call([sys.executable, "manage.py", "migrate"])
    except subprocess.CalledProcessError as e:
        sys.exit(f"Failed to apply database migrations. Error: {e}")


def run_django_server(script_dir: str) -> None:
    """Run the Django development server."""
    try:
        os.chdir(os.path.join(script_dir))
        subprocess.check_call([sys.executable, "manage.py", "runserver"])
    except subprocess.CalledProcessError as e:
        sys.exit(f"Failed to start Django server. Error: {e}")
    except KeyboardInterrupt:
        pass


def open_browser() -> None:
    """Open the web browser asynchronously."""
    webbrowser.open("http://127.0.0.1:8000")


if __name__ == "__main__":
    script_dir = os.path.dirname(os.path.abspath(__file__))
    install_requirements(script_dir)
    migrate_database(script_dir)
    threading.Thread(target=open_browser).start()
    run_django_server(script_dir)
