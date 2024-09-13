from datetime import datetime
from django.urls import path
from django.contrib.auth.views import LoginView, LogoutView
from . import forms, controllers


urlpatterns = [
    path('', controllers.home, name='home'),
    path('contact/', controllers.contact, name='contact'),
    path('about/', controllers.about, name='about'),
    path('login/',
         LoginView.as_view
         (
             template_name='login.html',
             authentication_form=forms.TailwindAuthenticationForm,
             extra_context={
                 'title': 'Log in',
                 'year': datetime.now().year,
             }
         ),
         name='login'),
    path('logout/', LogoutView.as_view(next_page='/'), name='logout'),
]
