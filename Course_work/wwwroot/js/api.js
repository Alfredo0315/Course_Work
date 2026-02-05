// ========================================
// API Configuration
// ========================================
// ВАЖНО: Измените этот URL на тот, который показывается в консоли Rider при запуске
// Обычно это https://localhost:7XXX или https://localhost:5001
const API_BASE_URL = 'http://localhost:5057/api';  // ← УБРАЛ S В HTTPS!

// ========================================
// API Service Class
// ========================================
class ApiService {
    constructor(baseUrl) {
        this.baseUrl = baseUrl;
    }

    // Generic GET request
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

    // Generic POST request
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

    // ========================================
    // News API
    // ========================================
    async getNews() {
        return this.get('/News');
    }

    async getNewsById(id) {
        return this.get(`/News/${id}`);
    }

    // ========================================
    // Games API
    // ========================================
    async getGames() {
        return this.get('/Games');
    }

    async getGameById(id) {
        return this.get(`/Games/${id}`);
    }

    // ========================================
    // Tournaments API
    // ========================================
    async getTournaments() {
        return this.get('/Tournaments');
    }

    async getTournamentById(id) {
        return this.get(`/Tournaments/${id}`);
    }

    // ========================================
    // Teams API
    // ========================================
    async getTeams() {
        return this.get('/Teams');
    }

    async getTeamById(id) {
        return this.get(`/Teams/${id}`);
    }

    async getTeamsByTournament(tournamentId) {
        return this.get(`/Teams/ByTournament/${tournamentId}`);
    }

    // ========================================
    // Players API
    // ========================================
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

    // ========================================
    // Matches API
    // ========================================
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

// ========================================
// Create Global Instance
// ========================================
const api = new ApiService(API_BASE_URL);

// ========================================
// Helper Functions
// ========================================

function formatDate(dateString) {
    if (!dateString) return 'N/A';
    const date = new Date(dateString);
    const options = { year: 'numeric', month: 'long', day: 'numeric' };
    return date.toLocaleDateString('ru-RU', options);
}

function formatTime(timeString) {
    if (!timeString) return '';
    const parts = timeString.split(':');
    return `${parts[0]}:${parts[1]}`;
}

function formatDateTime(dateTimeString) {
    if (!dateTimeString) return 'N/A';
    const date = new Date(dateTimeString);
    const dateOptions = { day: 'numeric', month: 'short' };
    const timeOptions = { hour: '2-digit', minute: '2-digit' };
    return `${date.toLocaleDateString('ru-RU', dateOptions)}, ${date.toLocaleTimeString('ru-RU', timeOptions)}`;
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

// ========================================
// Debugging Helper
// ========================================
console.log('API Service initialized with base URL:', API_BASE_URL);
console.log('Убедитесь что URL совпадает с портом вашего C# API!');