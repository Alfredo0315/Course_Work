// Main JavaScript for index.html

document.addEventListener('DOMContentLoaded', async () => {
    await loadNews();
    await loadUpcomingMatches();
    await loadTopTeams();
    await loadTopPlayers();
});

// Load News
async function loadNews() {
    try {
        showLoading('news-list');
        const news = await api.getNews();

        if (!news || news.length === 0) {
            document.getElementById('news-list').innerHTML =
                '<p class="error-message">Новостей пока нет</p>';
            return;
        }

        // Sort by date (newest first)
        news.sort((a, b) => new Date(b.date_of_publication) - new Date(a.date_of_publication));

        const newsHTML = news.map(item => createNewsCard(item)).join('');
        document.getElementById('news-list').innerHTML = newsHTML;
    } catch (error) {
        console.error('Error loading news:', error);
        showError('news-list', 'Не удалось загрузить новости');
    }
}

// Create News Card HTML
function createNewsCard(news) {
    const date = formatDate(news.date_of_publication);
    const time = formatTime(news.time_of_publication);

    return `
        <div class="news-card" onclick="openNewsDetail(${news.id_News})">
            <div class="news-card-content">
                <h3 class="news-card-title">${news.title}</h3>
                <div class="news-card-meta">
                    <span class="news-date">📅 ${date}</span>
                    <span class="news-time">🕐 ${time}</span>
                </div>
                <p class="news-card-description">${news.description || 'Описание отсутствует'}</p>
            </div>
        </div>
    `;
}

// Open News Detail (можно будет добавить модальное окно)
function openNewsDetail(newsId) {
    console.log('Opening news:', newsId);
    // TODO: Показать детали новости в модальном окне
}

// Load Upcoming Matches
async function loadUpcomingMatches() {
    try {
        showLoading('upcoming-matches');
        const matches = await api.getMatches();

        if (!matches || matches.length === 0) {
            document.getElementById('upcoming-matches').innerHTML =
                '<p style="color: var(--text-secondary); text-align: center;">Нет предстоящих матчей</p>';
            return;
        }

        // Filter upcoming and ongoing matches
        const now = new Date();
        const upcomingMatches = matches
            .filter(m => m.status === 'Запланирован' || m.status === 'Идет')
            .sort((a, b) => new Date(a.match_date) - new Date(b.match_date))
            .slice(0, 5);

        if (upcomingMatches.length === 0) {
            document.getElementById('upcoming-matches').innerHTML =
                '<p style="color: var(--text-secondary); text-align: center;">Нет предстоящих матчей</p>';
            return;
        }

        const matchesHTML = upcomingMatches.map(match => createMatchCard(match)).join('');
        document.getElementById('upcoming-matches').innerHTML = matchesHTML;
    } catch (error) {
        console.error('Error loading matches:', error);
        showError('upcoming-matches', 'Ошибка загрузки матчей');
    }
}

// Create Match Card HTML
function createMatchCard(match) {
    const dateTime = formatDateTime(match.match_date);
    const statusClass = match.status === 'Идет' ? 'live' : 'upcoming';

    return `
        <div class="match-item">
            <div class="match-tournament">🏆 Турнир #${match.id_Tournament}</div>
            <div class="match-date">📅 ${dateTime}</div>
            <span class="match-status ${statusClass}">${match.status}</span>
            ${match.score ? `<div style="margin-top: 5px; font-weight: 600;">Счёт: ${match.score}</div>` : ''}
        </div>
    `;
}

// Load Top Teams
async function loadTopTeams() {
    try {
        showLoading('top-teams');
        const teams = await api.getTeams();

        if (!teams || teams.length === 0) {
            document.getElementById('top-teams').innerHTML =
                '<p style="color: var(--text-secondary); text-align: center;">Нет данных о командах</p>';
            return;
        }

        // Sort by prize pool
        const topTeams = teams
            .sort((a, b) => (b.prize_pool || 0) - (a.prize_pool || 0))
            .slice(0, 10);

        const teamsHTML = topTeams.map((team, index) => createTeamCard(team, index + 1)).join('');
        document.getElementById('top-teams').innerHTML = teamsHTML;
    } catch (error) {
        console.error('Error loading teams:', error);
        showError('top-teams', 'Ошибка загрузки команд');
    }
}

// Create Team Card HTML
function createTeamCard(team, rank) {
    return `
        <div class="team-item">
            <div class="team-rank">#${rank}</div>
            <div class="team-info">
                <div class="team-name">${team.name}</div>
                <div class="team-country">🌍 ${team.country || 'N/A'}</div>
            </div>
            <div class="team-prize">${formatCurrency(team.prize_pool || 0)}</div>
        </div>
    `;
}

// Load Top Players
async function loadTopPlayers() {
    try {
        showLoading('top-players');
        const players = await api.getTopPlayersByPrize(10);

        if (!players || players.length === 0) {
            document.getElementById('top-players').innerHTML =
                '<p style="color: var(--text-secondary); text-align: center;">Нет данных об игроках</p>';
            return;
        }

        const playersHTML = players.map((player, index) => createPlayerCard(player, index + 1)).join('');
        document.getElementById('top-players').innerHTML = playersHTML;
    } catch (error) {
        console.error('Error loading players:', error);
        showError('top-players', 'Ошибка загрузки игроков');
    }
}

// Create Player Card HTML
function createPlayerCard(player, rank) {
    return `
        <div class="player-item">
            <div class="player-rank">#${rank}</div>
            <div class="player-info">
                <div class="player-nickname">${player.nickname}</div>
                <div class="player-team">👤 ${player.name || ''} ${player.surname || ''}</div>
            </div>
            <div class="player-prize">${formatCurrency(player.prize_pool || 0)}</div>
        </div>
    `;
}