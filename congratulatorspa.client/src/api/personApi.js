import { resumeToPipeableStream } from "react-dom/server";

export async function upcomingPeople({page = 1, pageSize = 10, sortBy, searchBy}) {
  const params = new URLSearchParams();

  params.append('page', page);
  params.append('pageSize', pageSize);

  if (sortBy) params.append('sortBy', sortBy);
  if (searchBy) params.append('searchBy', searchBy);

  const response = await fetch(`/api/person/main?${params.toString()}`);

  if (!response.ok) {
    const err = await response.json();
    throw new Error(err.error || 'Unknown error');
  }
  
  return response.json();
}

export async function getPeople({page = 1, pageSize = 10, sortBy, searchBy}) {
  const params = new URLSearchParams();

  params.append('page', page);
  params.append('pageSize', pageSize);

  if (sortBy) params.append('sortBy', sortBy);
  if (searchBy) params.append('searchBy', searchBy);

  const response = await fetch(`/api/person?${params.toString()}`);

  if (!response.ok) {
    const err = await response.json();
    throw new Error(err.error || 'Unknown error');
  }
  
  return response.json();
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