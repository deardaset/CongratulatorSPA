export async function getPeople({page = 1, pageSize = 10, sortBy, searchBy, upcoming}) {
  const params = new URLSearchParams();

  params.append('page', page);
  params.append('pageSize', pageSize);
  params.append('upcoming', upcoming);

  if (sortBy) params.append('sortBy', sortBy);
  if (searchBy) params.append('searchBy', searchBy);

  const response = await fetch(`/api/person?${params.toString()}`);

  if (!response.ok) {
    const jsonError = await response.json();
    if (jsonError.errors) {
      const messages = Object.values(jsonError.errors).flat();
      const error = new Error("Validation failed")
      error.messages = messages;
      throw error;
    }
  }
  
  return response.json();
}

export async function createPerson(formData) {
  const response = await fetch('/api/person', {
    method: 'POST',
    body: formData
  });

  if (!response.ok) {
    const jsonError = await response.json();
    if (jsonError.errors) {
      const messages = Object.values(jsonError.errors).flat();
      const error = new Error("Validation failed")
      error.messages = messages;
      throw error;
    }
  }
}

export async function updatePerson(guid, formData) {
  const response = await fetch(`/api/person/${guid}`, {
    method: 'PUT',
    body: formData
  });

  if (!response.ok) {
    const jsonError = await response.json();
    if (jsonError.errors) {
      const messages = Object.values(jsonError.errors).flat();
      const error = new Error("Validation failed")
      error.messages = messages;
      throw error;
    }
  }
}

export async function deletePerson(guid) {
  const response = await fetch(`/api/person/${guid}`, {
    method: 'DELETE'
  });

  if (!response.ok) {
    const jsonError = await response.json();
    if (jsonError.errors) {
      const messages = Object.values(jsonError.errors).flat();
      const error = new Error("Validation failed")
      error.messages = messages;
      throw error;
    }
  }
}