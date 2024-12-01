const connection = new signalR.HubConnectionBuilder()
    .withUrl("/presentationHub")
    .build()
    .connection.start().then(() => {
    console.log("Connected to SignalR Hub");
    });

let currentPresentationId = null;
let currentUserRole = "Viewer";
let nickname = prompt("Enter your nickname:");

connection.on("ReceiveUpdate", (update) => {
    // Handle real-time updates here
    console.log("Update received:", update);
    applyUpdate(update);
});

connection.on("UserJoined", (user) => {
    console.log("User joined:", user);
    addUserToList(user);
});

connection.on("RoleAssigned", (newRole) => {
    currentUserRole = newRole;
    alert(`Your role has been updated to: ${newRole}`);
});

// Triggered when user joins a presentation
function joinPresentation(presentationId, nickname) {
    currentPresentationId = presentationId;
    await connection.invoke("JoinPresentation", presentationId, nickname);
}

function applyUpdate(update) {
    // Example: Apply a text block update
    const { type, data } = update;
    if (type === "text-block") {
        const { id, content, position } = data;
        let textBlock = document.getElementById(id);
        if (!textBlock) {
            textBlock = document.createElement("div");
            textBlock.id = id;
            textBlock.className = "text-block";
            document.getElementById("activeSlide").appendChild(textBlock);
        }
        textBlock.textContent = content;
        textBlock.style.top = `${position.top}px`;
        textBlock.style.left = `${position.left}px`;
    }
}

function addUserToList(user) {
    const userList = document.getElementById("userList");
    const listItem = document.createElement("li");
    listItem.className = "list-group-item";
    listItem.textContent = `${user.nickname} (${user.role})`;
    userList.appendChild(listItem);
}

document.getElementById("addSlideBtn").addEventListener("click", () => {
    if (currentUserRole !== "Creator") {
        alert("Only the creator can add slides.");
        return;
    }

    const slideId = `slide-${Date.now()}`;
    const update = { type: "add-slide", slideId };
    connection.invoke("SendUpdate", currentPresentationId, update);
});

// Start the connection
connection.start().then(() => {
    console.log("Connected to SignalR hub.");
    const presentationId = prompt("Enter presentation ID to join:");
    joinPresentation(presentationId);
});

// Triggered when an editor modifies an element on a slide
function sendSlideUpdate(presentationId, updateData, userRole) {
    if (userRole === "Viewer") {
        console.error("Viewers cannot edit slides.");
        return;
    }

    connection.invoke("SendUpdate", presentationId, updateData, userRole)
        .then(() => {
            console.log("Update sent:", updateData);
        })
        .catch(err => console.error(err.toString()));
}

// Triggered when the creator assigns a role
//function assignRole(connectionId, role) {
//    connection.invoke("AssignRole", connectionId, role)
//        .then(() => {
//            console.log("Role assigned:", connectionId, role);
//        })
//        .catch(err => console.error(err.toString()));
//}

