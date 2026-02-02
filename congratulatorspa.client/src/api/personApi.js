//import

export async function getPeople() {
  const response = await fetch('/api/person');

  if (!response.ok) {
    throw new Error('Failed to fetch people');
  }

  return await response.json();
}