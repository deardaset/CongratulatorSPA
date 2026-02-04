import { resumeToPipeableStream } from "react-dom/server";

export async function getPeople() {
  const response = await fetch('/api/person');

  if (!response.ok) {
    throw new Error('Failed to fetch people');
  }

  return await response.json();
}

export async function createPerson(person) {
  const response = await fetch('/api/person', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(person)
  });

  if (!response.ok) {
    throw new Error('Failed to create person');
  }
}

export async function updatePerson(guid, data) {
  const response = await fetch(`/api/person/${guid}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(data)
  });

  if (!response.ok) {
    throw new Error('Failed to update person');
  }
}

export async function deletePerson(guid) {
  const response = await fetch(`/api/person/${guid}`, {
    method: 'DELETE'
  });

  if (!response.ok) {
    throw new Error('Failed to delete person');
  }
}