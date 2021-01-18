# Installation Script for Azure Kubernetes Cluster


@"
#                                   _____           _        _ _       _   _               
#       /\                         |_   _|         | |      | | |     | | (_)            
#      /  \    _____   _ _ __ ___    | |  _ __  ___| |_ __ _| | | __ _| |_ _  ___  _ __    
#     / /\ \  |_  / | | | '__/ _ \   | | | '_ \/ __| __/ _` | | |/ _` | __| |/ _ \| '_ \   
#    / ____ \  / /| |_| | | |  __/  _| |_| | | \__ \ || (_| | | | (_| | |_| | (_) | | | |
#   /_/    \_\/___|\__,_|_|  \___| |_____|_| |_|___/\__\__,_|_|_|\__,_|\__|_|\___/|_| |_|
#                                                                                        
#
#                                       #                                    
#                                    ###  ##*                                   
#                                 #####  #####                                  
#                               #####(  #######                                 
#                              #####   ##########                               
#                            ######      /########                              
#                           ######         .########                            
#                         #######             #######                           
#                                   /#################.
#
# Installation-Script by Julian Bücher            
"@


function Install() {
    
    Start-Sleep -s 5
    Write-Output "`n`nInstallation of Nginx Ingress: "

    helm install nginx-ingress ingress-nginx/ingress-nginx `
    --namespace ibsys-pps `
    --set controller.nodeSelector."beta\.kubernetes\.io/os"=linux `
    --set controller.replicaCount=1 `
    --set defaultBackend.nodeSelector."beta\.kubernetes\.io/os"=linux `
    --set controller.service.loadBalancerIP="20.52.204.244" `
    --set controller.service.annotations."service\.beta\.kubernetes\.io/azure-dns-label-name"="ibsystwopps-k8s"
    
    Start-Sleep -s 10
    Write-Output "`n`nInstallation of Cert Manager: "
    
    helm install cert-manager `
      --namespace ibsys-pps `
      --version v0.16.1 `
      --set installCRDs=true `
      --set nodeSelector."beta\.kubernetes\.io/os"=linux `
      jetstack/cert-manager
    
    Start-Sleep -s 40
    Write-Output "`n`nInstallation of Cluster Issuer: "
    
    kubectl apply -f .\cluster-issuer.yml

    Start-Sleep -s 5
    Write-Output "`n`nInstallation of Database: "
    
    kubectl apply -f .\postgres-configmap.yml
    kubectl apply -f .\postgres-storage.yml
    kubectl apply -f .\postgres-deployment.yml
    
    Start-Sleep -s 5
    Write-Output "`n`nInstallation of Applications: "
    
    kubectl apply -f .\pps-deployment.yml
    kubectl apply -f .\gui-deployment.yml
    kubectl apply -f .\elasticsearch-kibana-deployment.yml
    
    Start-Sleep -s 5
    Write-Output "`n`nInstallation of TLS Ingress: "
    
    kubectl apply -f .\tls-ingress-deployment.yml

    Write-Output "`n`nInstallation of ConfigMap for Nginx Ingress: "

    kubectl apply -f .\tls-ingress-configmap.yml

    Write-Output "`n`n"

    bash -c "cowsay -f tux Installation succeeded"
}

function Uninstall() {
    Write-Output "`n`nUninstall all existing Services, Deployments and Ingresses: `n"
    kubectl delete -n ibsys-pps svc,deployment,ingress,configmap --all

    Start-Sleep -s 5
    Write-Output "`n`nUninstall the Helm charts: `n"
    helm uninstall nginx-ingress cert-manager

    bash -c "cowsay -f tux Removal succeeded"
}

if ($args[0] -eq "Install") {
    Install
} elseif ($args[0] -eq "Uninstall") {
    Uninstall
} elseif ($args[0] -eq "Reinstall") {
    Uninstall && Install
} else {
    Write-Output "Wanted function is not available"
}
