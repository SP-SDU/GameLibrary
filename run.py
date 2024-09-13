import subprocess
import sys
import os
import webbrowser
import threading


def ensure_pkg_resources() -> None:
    """Ensure that pkg_resources is installed by installing setuptools if necessary."""  # noqa
    try:
        import pkg_resources  # noqa  # type: ignore
    except ImportError:
        subprocess.check_call([sys.executable, "-m", "pip", "install", "setuptools"])  # noqa


def install_requirements(script_dir: str) -> None:
    """Install missing packages from requirements.txt."""
    try:
        import pkg_resources  # type: ignore
        requirements_path = os.path.join(script_dir, "GameLibrary", "requirements.txt")  # noqa

        # Read the file with UTF-16 encoding to handle BOM and other issues
        with open(requirements_path, encoding='utf-16') as f:
            required = {pkg.split('==')[0].strip() for pkg in f if pkg.strip()}

        installed = {pkg.key for pkg in pkg_resources.working_set}
        missing = list(required - installed)  # Convert set to list

        if missing:
            # Debug: print missing packages
            print(f"Missing packages: {missing}")

            # Install packages individually
            for pkg in missing:
                print(f"Installing package: {pkg}")
                subprocess.check_call([sys.executable, "-m", "pip", "install", pkg])  # noqa
    except (FileNotFoundError, subprocess.CalledProcessError, UnicodeDecodeError) as e:  # noqa
        sys.exit(f"Failed to install packages or requirements.txt not found. Error: {e}")  # noqa


def run_django_server(script_dir: str) -> None:
    """Run the Django development server."""
    try:
        os.chdir(os.path.join(script_dir, "GameLibrary"))
        subprocess.check_call([sys.executable, "manage.py", "runserver"])
    except subprocess.CalledProcessError as e:
        sys.exit(f"Failed to start Django server. Error: {e}")
    except KeyboardInterrupt:
        print("\nDjango server stopped by user.")


def open_browser() -> None:
    """Open the web browser asynchronously."""
    webbrowser.open("http://127.0.0.1:8000")


if __name__ == "__main__":
    script_dir = os.path.dirname(os.path.abspath(__file__))
    ensure_pkg_resources()
    install_requirements(script_dir)
    threading.Thread(target=open_browser).start()
    run_django_server(script_dir)
