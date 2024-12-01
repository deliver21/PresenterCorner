document.getElementById('createUserForm').addEventListener('submit', async (event) => {
    event.preventDefault();

    const nickname = document.getElementById('nickname').value;

    const response = await fetch('/api/user/create-user', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(nickname),
    });

    if (response.ok) {
        const user = await response.json();
        console.log('User created or reused:', user);

        // Store user info for later (e.g., localStorage or in memory)
        localStorage.setItem('userNickname', user.Nickname);

        // Redirect to the main presentation page
        window.location.href = '/presentation/index';
    } else {
        console.error('Failed to create user');
    }
});
