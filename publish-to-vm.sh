#!/bin/bash
set -e

VM_USER="tim"
VM_HOST="192.168.50.199"
VM_DEPLOY_DIR="/opt/apps/RabbitComputerHelper"
PROJECT_DIR="/home/tim/Repos/RabbitComputerHelper/RabbitComputerHelper"
SERVICE_NAME="rabbitComputerHelper.service"

echo "Building..."
cd "$PROJECT_DIR"
dotnet publish -c Release -r linux-x64 --no-self-contained -o ./publish

echo "Ensuring ${VM_DEPLOY_DIR} exists on the VM..."
ssh -t ${VM_USER}@${VM_HOST} "sudo mkdir -p ${VM_DEPLOY_DIR} && sudo chown ${VM_USER}: ${VM_DEPLOY_DIR}"

echo "Copying to VM..."
rsync -avz --delete ./publish/ ${VM_USER}@${VM_HOST}:${VM_DEPLOY_DIR}/

echo "Restarting service..."
ssh -t ${VM_USER}@${VM_HOST} "sudo systemctl restart ${SERVICE_NAME}"

echo "Done."
