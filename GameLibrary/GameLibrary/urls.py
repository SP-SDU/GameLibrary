from django.contrib import admin
from django.urls import include, path

urlpatterns = [
    path('', include('app.urls')),  # Redirects to app urls
    path('admin/', admin.site.urls),
]
