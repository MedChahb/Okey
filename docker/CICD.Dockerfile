# syntax=docker/dockerfile:1

FROM python:3.10.13-bookworm

COPY requirements.txt requirements.txt

# Set up Python
# Install Python packages
RUN python -m pip install --upgrade pip && \
    pip install -r requirements.txt

# Set up .NET
RUN wget https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    rm packages-microsoft-prod.deb && \
    apt update && apt -y install dotnet-sdk-8.0=8.0.101-1

COPY .config/dotnet-tools.json .config/dotnet-tools.json

# Install .NET packages
RUN dotnet tool restore

CMD [ "/bin/bash" ]
