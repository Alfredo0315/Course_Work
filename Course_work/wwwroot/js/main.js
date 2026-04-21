let allNews = [];
let displayedNewsCount = 10;
let tournaments = {};
let games = {};

document.addEventListener('DOMContentLoaded', async () => {
    await loadTournamentsCache();
    await loadGamesCache();
    await loadAllNews();
    await loadUpcomingMatches();
    await loadTopTeams();
    await loadTopPlayers();
});


async function loadTournamentsCache() {
    try {
        tournaments = {};
        const tournamentsList = await api.getTournaments();
        if (tournamentsList && tournamentsList.length > 0) {
            tournamentsList.forEach(t => {
                tournaments[t.iD_Tournament] = t;
            });
        }
    } catch (error) {
        console.error('Error loading tournaments cache:', error);
    }
}


async function loadGamesCache() {
    try {
        games = {};
        const gamesList = await api.getGames();
        if (gamesList && gamesList.length > 0) {
            gamesList.forEach(g => {
                games[g.iD_Games] = g;
            });
        }
    } catch (error) {
        console.error('Error loading games cache:', error);
    }
}


function getTournamentName(tournamentId) {
    if (!tournamentId) return 'N/A';
    const tournament = tournaments[tournamentId];
    return tournament ? tournament.name : `Турнир #${tournamentId}`;
}


function getGameName(gameId) {
    if (!gameId) return 'N/A';
    const game = games[gameId];
    return game ? game.name : 'N/A';
}


async function loadAllNews() {
    try {
        showLoading('news-list');
        allNews = await api.getNews();

        if (!allNews || allNews.length === 0) {
            document.getElementById('news-list').innerHTML =
                '<p class="error-message">Новостей пока нет</p>';
            return;
        }

        allNews.sort((a, b) => {
            const dateA = new Date(a.date_of_publication + ' ' + a.time_of_publication);
            const dateB = new Date(b.date_of_publication + ' ' + b.time_of_publication);
            return dateB - dateA;
        });

        displayNews(allNews.slice(0, displayedNewsCount));
        updateShowMoreButton();
    } catch (error) {
        console.error('Error loading news:', error);
        showError('news-list', 'Не удалось загрузить новости');
    }
}


function displayNews(newsToShow) {
    const newsHTML = newsToShow.map(item => createNewsCard(item)).join('');
    const container = document.getElementById('news-list');
    container.innerHTML = newsHTML;
}


function updateShowMoreButton() {
    const showMoreBtn = document.getElementById('show-more-btn');
    if (displayedNewsCount >= allNews.length) {
        if (showMoreBtn) showMoreBtn.style.display = 'none';
    } else {
        if (showMoreBtn) showMoreBtn.style.display = 'block';
    }
}


function showMoreNews() {
    displayedNewsCount += 10;
    displayNews(allNews.slice(0, displayedNewsCount));
    updateShowMoreButton();
}


function filterNewsByDate(selectedDate) {
    if (!selectedDate) {
        displayNews(allNews.slice(0, displayedNewsCount));
        updateShowMoreButton();
        return;
    }
    
    const filtered = allNews.filter(news => {
        const newsDate = news.date_of_publication || '';
        return newsDate.toString().includes(selectedDate);
    });

    if (filtered.length === 0) {
        document.getElementById('news-list').innerHTML =
            '<p class="error-message">Новостей за эту дату не найдено</p>';
    } else {
        displayNews(filtered);
    }

    const showMoreBtn = document.getElementById('show-more-btn');
    if (showMoreBtn) showMoreBtn.style.display = 'none';
}


function createNewsCard(news) {
    const date = formatDate(news.date_of_publication);
    const time = formatTime(news.time_of_publication);

    return `
        <div class="news-card">
            <div class="news-card-content">
                <h3 class="news-card-title">${news.title}</h3>
                <div class="news-card-meta">
                    <span class="news-date"> ${date}</span>
                    <span class="news-time"> ${time}</span>
                </div>
                <p class="news-card-description">${news.description || 'Описание отсутствует'}</p>
            </div>
        </div>
    `;
}


async function loadUpcomingMatches() {
    try {
        showLoading('upcoming-matches');
        const matches = await api.getUpcomingMatches();
        console.log('Upcoming matches:', matches);
        if (matches && matches.length > 0) {
            console.log('First match:', matches[0]);
            console.log('match_Time:', matches[0].match_Time);
            console.log('match_time:', matches[0].match_time);
        }
        if (!matches || matches.length === 0) {
            document.getElementById('upcoming-matches').innerHTML =
                '<p style="color: var(--text-secondary); text-align: center;">Нет предстоящих матчей</p>';
            return;
        }

        const matchesHTML = matches.map(match => createMatchCard(match)).join('');
        document.getElementById('upcoming-matches').innerHTML = matchesHTML;
    } catch (error) {
        console.error('Error loading matches:', error);
        showError('upcoming-matches', 'Ошибка загрузки матчей');
    }
}


function createMatchCard(match) {
    const dateTime = formatDateTime(match.match_date);
    const statusClass = match.status === 'Идет' ? 'live' : 'upcoming';
    const tournamentName = match.tournamentName || 'N/A';

    return `
        <div class="match-item">
            <div class="match-tournament">🏆 ${tournamentName}</div>
            <div class="match-date"> ${dateTime}</div>
            <span class="match-status ${statusClass}">${match.status}</span>
            ${match.score ? `<div style="margin-top: 5px; font-weight: 600;">Счёт: ${match.score}</div>` : ''}
        </div>
    `;
}


async function loadTopTeams() {
    try {
        showLoading('top-teams');
        const teams = await api.getTeams();

        if (!teams || teams.length === 0) {
            document.getElementById('top-teams').innerHTML =
                '<p style="color: var(--text-secondary); text-align: center;">Нет данных о командах</p>';
            return;
        }

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


function createPlayerCard(player, rank) {
    return `
        <div class="player-item">
            <div class="player-rank">#${rank}</div>
            <div class="player-info">
                <div class="player-nickname">${player.nickname}</div>
                <div class="player-name">👤 ${player.name || ''} ${player.surname || ''}</div>
                <div class="player-team" style="color: #ff4757; font-size: 0.9em;">
                     ${player.teamName || 'Без команды'}
                </div>
            </div>
            <div class="player-prize">${formatCurrency(player.prize_pool || 0)}</div>
        </div>
    `;
}


async function loadTournamentsPage() {
    try {
        showLoading('tournaments-list');
        const tournaments = await api.getTournaments();

        if (!tournaments || tournaments.length === 0) {
            document.getElementById('tournaments-list').innerHTML =
                '<p style="color: var(--text-secondary); text-align: center;">Нет данных о турнирах</p>';
            return;
        }

        const tournamentsHTML = tournaments.map(t => createTournamentCard(t)).join('');
        document.getElementById('tournaments-list').innerHTML = tournamentsHTML;
    } catch (error) {
        console.error('Error loading tournaments:', error);
        showError('tournaments-list', 'Ошибка загрузки турниров');
    }
}


function createTournamentCard(tournament) {
    return `
        <div class="tournament-card">
            <div class="tournament-header">
                <h3 class="tournament-name">${tournament.name}</h3>
            </div>
            <div class="tournament-details">
                <div class="tournament-game">
                     <strong>Дисциплина:</strong> ${tournament.gameName || 'N/A'}
                </div>
                <div class="tournament-genre">
                     <strong>Жанр:</strong> ${tournament.genre || 'N/A'}
                </div>
                <div class="tournament-platform">
                     <strong>Платформа:</strong> ${tournament.platform || 'N/A'}
                </div>
                <div class="tournament-date">
                     <strong>Начало:</strong> ${formatDate(tournament.start_date)}
                </div>
                <div class="tournament-date">
                    <strong>Окончание:</strong> ${formatDate(tournament.end_date)}
                </div>
                <div class="tournament-location">
                     <strong>Страна:</strong> ${tournament.location_of_the_event || 'N/A'}
                </div>
                <div class="tournament-prize">
                     <strong>Призовой фонд:</strong> ${formatCurrency(tournament.prize_pool || 0)}
                </div>
            </div>
        </div>
    `;
}


async function loadMatchesPage() {
    try {
        showLoading('matches-list');
        const matches = await api.getMatches();

        if (!matches || matches.length === 0) {
            document.getElementById('matches-list').innerHTML =
                '<p style="color: var(--text-secondary); text-align: center;">Нет данных о матчах</p>';
            return;
        }

        const matchesHTML = matches.map(m => createMatchRow(m)).join('');
        document.getElementById('matches-list').innerHTML = matchesHTML;
    } catch (error) {
        console.error('Error loading matches:', error);
        showError('matches-list', 'Ошибка загрузки матчей');
    }
}


function createMatchRow(match) {
    const dateTime = formatDateTime(match.match_date);
    const teamsList = match.teams ? match.teams.map(t => t.name).join(' vs ') : 'N/A';

    return `
        <tr>
            <td>${dateTime}</td>
            <td>${match.tournamentName || 'N/A'}</td>
            <td>${teamsList}</td>
            <td><span class="status-${match.status === 'Идет' ? 'live' : 'scheduled'}">${match.status}</span></td>
            <td>${match.score || '-'}</td>
        </tr>
    `;
}


async function loadTeamsPage() {
    try {
        showLoading('teams-list');
        const teams = await api.getTeams();

        if (!teams || teams.length === 0) {
            document.getElementById('teams-list').innerHTML =
                '<p style="color: var(--text-secondary); text-align: center;">Нет данных о командах</p>';
            return;
        }

        const teamsHTML = teams.map(t => createTeamDetailCard(t)).join('');
        document.getElementById('teams-list').innerHTML = teamsHTML;
    } catch (error) {
        console.error('Error loading teams:', error);
        showError('teams-list', 'Ошибка загрузки команд');
    }
}


function createTeamDetailCard(team) {
    return `
        <div class="team-detail-card">
            <h3 class="team-name">${team.name}</h3>
            <div class="team-info">
                <p> <strong>Страна:</strong> ${team.country || 'N/A'}</p>
                <p> <strong>Год основания:</strong> ${team.founded_year || 'N/A'}</p>
                <p> <strong>Призовые:</strong> ${formatCurrency(team.prize_pool || 0)}</p>
            </div>
        </div>
    `;
}


async function loadPlayersPage() {
    try {
        showLoading('players-list');
        const players = await api.getPlayers();

        if (!players || players.length === 0) {
            document.getElementById('players-list').innerHTML =
                '<p style="color: var(--text-secondary); text-align: center;">Нет данных об игроках</p>';
            return;
        }

        const playersHTML = players.map(p => createPlayerDetailRow(p)).join('');
        document.getElementById('players-list').innerHTML = playersHTML;
    } catch (error) {
        console.error('Error loading players:', error);
        showError('players-list', 'Ошибка загрузки игроков');
    }
}


function createPlayerDetailRow(player) {
    return `
        <tr>
            <td>${player.nickname}</td>
            <td>${player.name || ''} ${player.surname || ''}</td>
            <td>${player.country || 'N/A'}</td>
            <td>${player.teamName || 'Без команды'}</td>
            <td>${formatCurrency(player.prize_pool || 0)}</td>
        </tr>
    `;
}