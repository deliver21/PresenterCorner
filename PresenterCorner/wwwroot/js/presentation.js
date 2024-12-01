document.addEventListener("DOMContentLoaded", () => {
    const presentationsList = document.getElementById("presentationsList");
    const nicknameModal = new bootstrap.Modal(document.getElementById("nicknameModal"));
    let selectedPresentationId = null;

    // Fetch and display presentations
    function loadPresentations() {
        fetch("/api/presentation/list")
            .then(res => res.json())
            .then(presentations => {
                presentationsList.innerHTML = presentations.map(p => `
                    <li class="list-group-item">
                        <span>${p.title}</span>
                        <button class="btn btn-link float-end join-btn" data-id="${p.id}">Join</button>
                    </li>
                `).join("");
            });
    }

    document.getElementById('createPresentationForm').addEventListener('submit', async (event) => {
        event.preventDefault();

        const title = document.getElementById('presentationTitle').value;
        const nickname = localStorage.getItem('userNickname');

        const response = await fetch('/api/presentation/create-presentation', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ title, nickname }),
        });

        if (response.ok) {
            const presentation = await response.json();
            console.log('Presentation created:', presentation);

            // Redirect to the presentation page
            window.location.href = `/presentation/${presentation.Id}`;
        } else {
            console.error('Failed to create presentation');
        }
    });


    // Show nickname modal
    presentationsList.addEventListener("click", (e) => {
        if (e.target.classList.contains("join-btn")) {
            selectedPresentationId = e.target.dataset.id;
            nicknameModal.show();
        }
    });

    // Join presentation
    document.getElementById("joinBtn").addEventListener("click", () => {
        const nickname = document.getElementById("nickname").value;
        fetch(`/api/presentation/${selectedPresentationId}/join`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(nickname)
        }).then(res => {
            if (res.ok) {
                window.location.href = `/presentation/${selectedPresentationId}`;
            } else {
                alert("Nickname already in use. Choose a different one.");
            }
        });
    });

    loadPresentations();
});
