# Grupo2-MiniERP
Mini ERP para microempresa  Módulos: clientes, proveedores, compras, ventas, inventario, cuentas por cobrar/pagar, y reportes. Requiere reglas fuertes de negocio y auditoría de cambios.

Configuración Inicial
Instala Git y configura tu identidad global una sola vez.

git config --global user.name "Tu Nombre" establece tu nombre de usuario.​
git config --global user.email "tu@email.com" define tu correo para commits.​
git config --list verifica la configuración actual.
​

Comandos Básicos Locales
Inicia y gestiona un repositorio local.

git init crea un repositorio Git en la carpeta actual.
git status muestra el estado de archivos modificados, staged o untracked.
git add <archivo> o git add . agrega cambios al área de staging.
git commit -m "Mensaje descriptivo" guarda los cambios con un mensaje.
git log o git log --oneline ve el historial de commits.
​

SSH para Repositorios Remotos
SSH autentica de forma segura sin contraseñas; genera claves en ~/.ssh (usa Git Bash en Windows).

ssh-keygen -t rsa -b 4096 -C "tu@email.com" genera un par de claves (presiona Enter para ruta por defecto y sin passphrase inicial).
eval $(ssh-agent -s) inicia el agente SSH.
ssh-add ~/.ssh/id_rsa agrega tu clave privada al agente.
cat ~/.ssh/id_rsa.pub copia la clave pública y agrégala a GitHub (Settings > SSH keys).
ssh -T git@github.com prueba la conexión (debe decir "Hi username!").
​

Clonar y Remotos
Conecta con repositorios remotos usando SSH (ej: git@github.com:usuario/repo.git).

git clone <URL-SSH> copia un repositorio remoto.
git remote add origin <URL-SSH> vincula un remoto.
git remote -v lista los remotos configurados.
​

Ramas (Branches)
Gestiona flujos de trabajo como feature branches en tu internship.

git branch lista ramas locales.
git branch <nombre-rama> crea una rama.
git checkout <rama> o git switch <rama> cambia a una rama.
git checkout -b <rama> crea y cambia a una nueva rama.
git merge <rama> fusiona cambios de otra rama.
​

Push y Pull
Sincroniza con remotos; usa SSH para pushes seguros.

git push origin <rama> envía commits al remoto.
git pull origin <rama> trae y fusiona cambios remotos.
git push -u origin <rama> push inicial con tracking.
git push origin --delete <rama> elimina rama remota.
​
