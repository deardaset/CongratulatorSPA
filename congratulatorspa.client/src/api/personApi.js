import { resumeToPipeableStream } from "react-dom/server";

export async function getPeople(page, pageSize) {
  const response = await fetch(
    `/api/person?page=${page}&pageSize=${pageSize}`
  );

  if (!response.ok) {
    const err = await response.json();
    throw new Error(err.erro || 'Unknown error');
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
    const jsonError = await response.json();
    throw new Error(jsonError.error || 'Unknown error')
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
    const jsonError = await response.json();
    throw new Error(jsonError.error || 'Unknown error')
  }
}

export async function deletePerson(guid) {
  const response = await fetch(`/api/person/${guid}`, {
    method: 'DELETE'
  });

  if (!response.ok) {
    const jsonError = await response.json();
    throw new Error(jsonError.error || 'Unknown error')
  }
}