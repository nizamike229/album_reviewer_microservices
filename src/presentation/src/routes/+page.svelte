<script lang="ts">
	import axios from 'axios';

	let query = '';
	let albums: string | any[] = [];
	let loading = false;

	type Album = {
		title: string;
		artist: string;
		mbid: string;
		cover: string;
	};

	type MusicBrainzItem = {
		id: string;
		title: string;
		'artist-credit'?: Array<{ name: string }>;
	};

	async function search() {
		albums = [];
		loading = true;

		if (!query.trim()) {
			loading = false;
			return;
		}

		try {
			const res = await axios.get('https://musicbrainz.org/ws/2/release-group/', {
				params: {
					query: `release:${query}`,
					type: 'album',
					fmt: 'json'
				},
				headers: {
					'User-Agent': 'YourApp/1.0 (youremail@example.com)'
				}
			});

			const items = res.data['release-groups'];

			const coversLoaded = items.map(async (item: MusicBrainzItem) => {
				const mbid = item.id;
				const coverUrl = `https://coverartarchive.org/release-group/${mbid}/front`;
				try {
					await axios.head(coverUrl);
					return {
						title: item.title,
						artist: item['artist-credit']?.[0]?.name ?? 'Unknown',
						mbid,
						cover: coverUrl
					};
				} catch {
					return null;
				}
			});

			albums = (await Promise.all(coversLoaded)).filter(Boolean);
		} catch (err) {
			console.error('Ошибка при поиске:', err);
		} finally {
			loading = false;
		}
	}

	const handleRedirectToAlbum = (mbid: string) => {
		window.location.href = `/album/${mbid}`;
	};
</script>

<div class="min-h-screen bg-zinc-900 text-zinc-100 px-4 py-8 font-mono">
	<h1 class="text-4xl font-bold mb-8 text-center font-mono">🎧 Поиск альбомов</h1>

	<div class="max-w-3xl mx-auto mb-10">
		<input
			type="text"
			bind:value={query}
			on:keydown={(e: KeyboardEvent) => e.key === 'Enter' && search()}
			placeholder="Введите название альбома…"
			class="w-full bg-zinc-800 text-white text-xl px-5 py-4 rounded-2xl outline-none focus:ring-2 focus:ring-zinc-400 transition font-mono"
		/>
	</div>

	<div class="max-w-5xl mx-auto space-y-6 min-h-[200px] font-mono grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
		{#if loading}
			<div class="flex justify-center items-center py-20 col-span-full">
				<div class="animate-spin inline-block w-12 h-12 border-4 border-zinc-500 border-t-zinc-200 rounded-full"></div>
			</div>
		{:else if albums.length === 0 && query}
			<p class="text-center text-zinc-400 col-span-full">Ничего не найдено или нет обложек.</p>
		{:else}
			{#each albums as album (album.mbid)}
				<!-- svelte-ignore a11y_click_events_have_key_events -->
				<!-- svelte-ignore a11y_no_static_element_interactions -->
				<button class="bg-zinc-800 hover:bg-zinc-700 rounded-2xl p-6 shadow transition border border-zinc-700 font-mono flex flex-col items-center w-full h-[320px] min-h-[320px] max-h-[340px] min-w-[260px] max-w-[340px] mx-auto" on:click={handleRedirectToAlbum.bind(null, album.mbid)}>
					<img
						src={album.cover}
						alt="Обложка"
						class="w-32 h-32 object-cover rounded-xl shadow-lg mb-4"
					/>
					<h2 class="text-2xl font-semibold text-zinc-100 font-mono text-center truncate w-full">{album.title}</h2>
					<p class="text-zinc-400 text-lg font-mono text-center truncate w-full">{album.artist}</p>
				</button>
			{/each}
		{/if}
	</div>
</div>