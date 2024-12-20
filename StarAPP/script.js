// URL da API
const apiUrl = 'https://localhost:44317/api/Items';

// Seleciona os contêineres
const cardsContainer = document.getElementById('cards-container');
const progressFill = document.querySelector('.progress-fill');
const progressText = document.querySelector('.progress-text');

// Função para criar um card
function createCard(item) {
    const card = document.createElement('div');
    card.className = 'card';

    const button = document.createElement('button');
    button.textContent = item.isCompleted ? 'Marcar como não concluído' : 'Marcar como concluído!';

    // Define a cor do botão dependendo do estado
    if (item.isCompleted) {
        button.style.backgroundColor = '#2b783c';  // Cor 1 para concluído
    } else {
        button.style.backgroundColor = '#273B6D';  // Cor 2 para não concluído
    }

    // Adiciona o evento de clique para alternar o estado do item
    button.addEventListener('click', () => toggleItemCompletion(item, button));

    card.innerHTML = `
        <img src="${item.imageUri}" alt="${item.name}">
        <h3>${item.name}</h3>
        <p id="category_name">${item.categoryName}</p>
        <p>${item.description}</p>
    `;
    
    card.appendChild(button);

    return card;
}

// Função para carregar os itens da API
async function loadItems() {
    try {
        const response = await fetch(apiUrl, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            },
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const items = await response.json();
        const totalItems = items.length;
        let completedItems = 0;

        // Adiciona os cards no contêiner
        cardsContainer.innerHTML = ''; // Limpa o contêiner antes de adicionar novos itens
        items.forEach(item => {
            const card = createCard(item);
            cardsContainer.appendChild(card);
            if (item.isCompleted) completedItems++;
        });

        // Atualiza o progresso
        updateProgress(completedItems, totalItems);
    } catch (error) {
        console.error('Erro ao carregar os itens:', error);
        cardsContainer.innerHTML = '<p>Erro ao tentar carregar os itens.</p>';
    }
}

// Função para alternar o estado de conclusão de um item
async function toggleItemCompletion(item, button) {
    try {
        const newCompletionStatus = !item.isCompleted;

        const response = await fetch(`${apiUrl}/${item.id}`, {
            method: 'PATCH',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify([{
                op: "replace", // Operação de substituição
                path: "/isCompleted", // Caminho para a propriedade
                value: newCompletionStatus // Novo valor para a propriedade
            }])
        });

        if (!response.ok) {
            throw new Error(`HTTP error ao marcar como concluído/desmarcado. Status: ${response.status}`);
        }

        // Atualiza o estado do item
        item.isCompleted = newCompletionStatus;

        // Atualiza a cor do botão e o texto com base no novo status
        if (newCompletionStatus) {
            button.style.backgroundColor = '#2b783c';  // Cor 1 para concluído
            button.textContent = 'Marcar como não concluído'; // Atualiza o texto do botão
        } else {
            button.style.backgroundColor = '#273B6D';  // Cor 2 para não concluído
            button.textContent = 'Marcar como concluído!'; // Atualiza o texto do botão
        }

        // Atualiza o progresso sem recarregar os itens
        updateProgress();
    } catch (error) {
        console.error('Erro ao alternar o estado de conclusão do item:', error);
    }
}

// Função para atualizar o progresso
function updateProgress() {
    const cards = cardsContainer.getElementsByClassName('card');
    const completedItems = Array.from(cards).filter(card => card.querySelector('button').style.backgroundColor === 'rgb(43, 120, 60)').length; // Cor 1 para concluído
    const totalItems = cards.length;

    const progress = (completedItems / totalItems) * 100;
    progressFill.style.width = `${progress}%`;
    progressText.textContent = `${Math.round(progress)}%`;
}

// Inicializa o carregamento dos itens
loadItems();
