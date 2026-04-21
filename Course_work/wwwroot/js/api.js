//const API_BASE_URL = 'http://localhost:5057/api';

class ApiService {
    constructor(baseUrl) {
        this.baseUrl = baseUrl;
    }

    // ============================================
    // ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ
    // ============================================

    async get(endpoint) {
        try {
            const response = await fetch(`${this.baseUrl}${endpoint}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                }
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            return await response.json();
        } catch (error) {
            console.error('API GET Error:', error);
            throw error;
        }
    }

    async post(endpoint, data) {
        try {
            const response = await fetch(`${this.baseUrl}${endpoint}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(data),
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            return await response.json();
        } catch (error) {
            console.error('API POST Error:', error);
            throw error;
        }
    }

    async delete(endpoint) {
        try {
            const response = await fetch(`${this.baseUrl}${endpoint}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                }
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            // 204 No Content — сервер не возвращает тело, json() упадёт
            if (response.status === 204 || response.headers.get('content-length') === '0') {
                return { success: true };
            }

            const text = await response.text();
            return text ? JSON.parse(text) : { success: true };
        } catch (error) {
            console.error('API DELETE Error:', error);
            throw error;
        }
    }

    // ============================================
    // АВТОРИЗАЦИЯ (НОВОЕ)
    // ============================================

    async login(login, password) {
        return this.post('/auth/login', { login, password });
    }

    async register(login, password, role, name, email) {
        return this.post('/auth/register', { login, password, role, name, email });
    }

    async getUsers() {
        return this.get('/auth/users');
    }

    async deleteUser(id) {
        return this.delete(`/auth/users/${id}`);
    }

    // ============================================
    // НОВОСТИ
    // ============================================

    async getNews() {
        return this.get('/News');
    }

    async getNewsById(id) {
        return this.get(`/News/${id}`);
    }

    // ============================================
    // ИГРЫ
    // ============================================

    async getGames() {
        return this.get('/Games');
    }

    async getGameById(id) {
        return this.get(`/Games/${id}`);
    }

    // ============================================
    // ТУРНИРЫ
    // ============================================

    async getTournaments() {
        return this.get('/Tournaments');
    }

    async getTournamentById(id) {
        return this.get(`/Tournaments/${id}`);
    }

    // ============================================
    // КОМАНДЫ
    // ============================================

    async getTeams() {
        return this.get('/Teams');
    }

    async getTeamById(id) {
        return this.get(`/Teams/${id}`);
    }

    async getTeamsByTournament(tournamentId) {
        return this.get(`/Teams/ByTournament/${tournamentId}`);
    }

    // ============================================
    // ИГРОКИ
    // ============================================

    async getPlayers() {
        return this.get('/Players');
    }

    async getPlayerById(id) {
        return this.get(`/Players/${id}`);
    }

    async getPlayersByTeam(teamId) {
        return this.get(`/Players/ByTeam/${teamId}`);
    }

    async getPlayersByCountry(country) {
        return this.get(`/Players/ByCountry/${country}`);
    }

    async getTopPlayersByPrize(count = 10) {
        return this.get(`/Players/TopByPrize?count=${count}`);
    }

    // ============================================
    // МАТЧИ
    // ============================================

    async getMatches() {
        return this.get('/Matches');
    }

    async getMatchById(id) {
        return this.get(`/Matches/${id}`);
    }

    async getMatchesByTournament(tournamentId) {
        return this.get(`/Matches/ByTournament/${tournamentId}`);
    }

    async getUpcomingMatches() {
        return this.get('/Matches/Upcoming');
    }
}

const api = new ApiService(API_BASE_URL);

// ============================================
// УТИЛИТЫ
// ============================================

function formatDate(dateString) {
    if (!dateString) return 'N/A';
    const datePart = dateString.toString().substring(0, 10);
    const [year, month, day] = datePart.split('-').map(Number);
    const months = ['января', 'февраля', 'марта', 'апреля', 'мая', 'июня',
        'июля', 'августа', 'сентября', 'октября', 'ноября', 'декабря'];
    return `${day} ${months[month - 1]} ${year}`;
}

function formatTime(timeString) {
    if (!timeString) return '';
    const parts = timeString.split(':');
    return `${parts[0]}:${parts[1]}`;
}

function formatDateTime(dateTimeString) {
    if (!dateTimeString) return 'N/A';
    const str = dateTimeString.toString();
    const datePart = str.substring(0, 10);
    const [year, month, day] = datePart.split('-').map(Number);
    let timePart = '';
    const tIndex = str.indexOf('T');
    const spaceIndex = str.indexOf(' ');
    const sepIndex = tIndex !== -1 ? tIndex : spaceIndex;
    if (sepIndex !== -1 && str.length > sepIndex + 1) {
        timePart = str.substring(sepIndex + 1, sepIndex + 6);
    }
    const months = ['янв', 'фев', 'мар', 'апр', 'мая', 'июн',
        'июл', 'авг', 'сен', 'окт', 'ноя', 'дек'];
    const formattedDate = `${day} ${months[month - 1]}`;
    if (timePart) {
        return `${formattedDate}, ${timePart}`;
    }
    return formattedDate;
}

function formatCurrency(amount) {
    if (!amount && amount !== 0) return '$0';
    return new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
        minimumFractionDigits: 0,
        maximumFractionDigits: 0,
    }).format(amount);
}

function showLoading(elementId) {
    const element = document.getElementById(elementId);
    if (element) {
        element.innerHTML = '<div class="loading">Загрузка...</div>';
    }
}

function showError(elementId, message = 'Ошибка загрузки данных') {
    const element = document.getElementById(elementId);
    if (element) {
        element.innerHTML = `<div class="error-message">${message}</div>`;
    }
}

console.log('API Service initialized with base URL:', API_BASE_URL);