//const API_BASE_URL = 'http://localhost:5057/api';

const ROLE_LABELS = {
    Admin: 'Администратор',
    ContentManager: 'Контент-менеджер',
    User: 'Пользователь',
    guest: 'Гость',
};

const ROLE_COLORS = {
    Admin: '#ff4757',
    ContentManager: '#3498db',
    User: '#2ecc71',
    guest: '#888888',
};

const AuthService = {
    async login(username, password) {
        try {
            console.log('Trying login:', username, 'password:', password); //удалить
            const response = await fetch(`${API_BASE_URL}/auth/login`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ login: username, password: password })
            });
            console.log('Response status:', response.status); // удалить
            if (response.ok) {
                const user = await response.json();
                console.log('User data:', user); //удалить
                localStorage.setItem('auth_user', JSON.stringify({
                    id: user.id,
                    username: user.login,
                    role: user.role,
                    displayName: user.name || user.login,
                }));
                return { success: true, user };
            }
            return { success: false };
        } catch (error) {
            console.error('Login error:', error);
            return { success: false };
        }
    },

    logout() {
        localStorage.removeItem('auth_user');
        window.location.href = this._loginPath();
    },

    isLoggedIn() {
        return !!localStorage.getItem('auth_user');
    },

    getCurrentUser() {
        const raw = localStorage.getItem('auth_user');
        if (!raw) return { role: 'guest', displayName: 'Гость', username: '' };
        try { return JSON.parse(raw); } catch { return { role: 'guest', displayName: 'Гость', username: '' }; }
    },

    getRole() {
        return this.getCurrentUser().role;
    },

    isAdmin()   { return this.getRole() === 'Admin'; },
    isManager() { return this.getRole() === 'ContentManager'; },
    isUser()    { return this.getRole() === 'User'; },
    isGuest()   { return this.getRole() === 'guest'; },

    canManageNews() {
        return this.isAdmin() || this.isManager();
    },

    canManageMatches() {
        return this.isAdmin() || this.isManager();
    },

    canManageUsers()   { return this.isAdmin(); },
    canManagePlayers() { return this.isAdmin(); },
    canManageTeams()   { return this.isAdmin(); },
    canManageTournaments() { return this.isAdmin(); },

    applyVisibility() {
        const role = this.getRole();
        const loggedIn = this.isLoggedIn();

        document.querySelectorAll('[data-role]').forEach(el => {
            const allowed = el.getAttribute('data-role').split(',').map(s => s.trim());
            let visible = false;

            if (allowed.includes('auth'))  visible = loggedIn;
            else if (allowed.includes('guest')) visible = !loggedIn;
            else visible = allowed.includes(role);

            el.style.display = visible ? '' : 'none';
        });
    },

    renderAuthWidget() {
        const container = document.getElementById('auth-widget');
        if (!container) return;

        const user = this.getCurrentUser();
        const color = ROLE_COLORS[user.role] || '#888';
        const label = ROLE_LABELS[user.role] || 'Гость';

        if (this.isLoggedIn()) {
            container.innerHTML = `
                <div class="auth-user">
                    <span class="auth-role-badge" style="background:${color}22;color:${color};border:1px solid ${color}55;">
                        ${label}
                    </span>
                    <span class="auth-name">${user.displayName}</span>
                    <button class="auth-logout-btn" onclick="AuthService.logout()">Выйти</button>
                </div>
            `;
        } else {
            container.innerHTML = `
                <a href="${this._loginPath()}" class="auth-login-link">Войти</a>
            `;
        }
    },

    _loginPath() {
        const isSubpage = window.location.pathname.includes('/pages/');
        return isSubpage ? 'login.html' : 'pages/login.html';
    }
};